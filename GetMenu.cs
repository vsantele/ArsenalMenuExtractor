using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Vsantele.Functions.Models;

namespace Vsantele.Functions
{
    public class GetMenu
    {
        [FunctionName("GetMenu")]
        [return: CosmosDB(
            databaseName: "arsenal",
            collectionName: "menu",
            ConnectionStringSetting = "CosmosDbConnectionString")]
        public static async Task<Menu> Run([TimerTrigger("0 0 18 * * 0")] TimerInfo myTimer, ILogger log)
        {
            var html = await CallUrl("https://www.unamur.be/services/vecu/arsenal-restaurants-salles/menu-tarif");
            var imageLink = GetImageLink(html);
            var menu = await ExtractInformation(imageLink);
            var weekInfo = GetWeekInfo(html);
            var menuInfo = new Menu
            {
                Id = weekInfo.WeekNumber,
                WeekInfo = weekInfo,
                MenuDetails = menu
            };
            return menuInfo;
        }


        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        private static WeekInfo GetWeekInfo(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var weekTitle = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "").Contains("content-core"))
                .First().Descendants("h2").First().InnerText;
            var regex = @"MENU DE LA SEMAINE (?:#)(\d+) \| DU (\d+)((?: )(\w+))? au (\d+) (\w+) (\d+)";
            var rg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var match = rg.Match(weekTitle);
            var weekNumber = match.Groups[1].Value;
            var dayStart = match.Groups[2].Value;
            var monthStart = match.Groups[4].Value;
            var dayEnd = match.Groups[5].Value;
            var monthEnd = match.Groups[6].Value;
            var year = match.Groups[7].Value;

            if (monthStart == "") monthStart = monthEnd;

            return new WeekInfo
            {
                WeekNumber = weekNumber,
                DayStart = dayStart,
                MonthStart = monthStart,
                DayEnd = dayEnd,
                MonthEnd = monthEnd,
                Year = year
            };

        }

        private static string GetImageLink(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var imageLink = htmlDoc.DocumentNode.Descendants("img")
                .Where(node => node.GetAttributeValue("class", "").Contains("image-inline"))
                .First().GetAttributeValue("src", "");
            return imageLink;
        }


        private static async Task<List<List<string>>> ExtractInformation(string imageLink)
        {
            // get azure form recognizer endpoint from local config
            string endpoint = Environment.GetEnvironmentVariable("AzureFormRecognizerEndpoint");
            string apiKey = Environment.GetEnvironmentVariable("AzureFormRecognizerApiKey");
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            Uri fileUri = new(imageLink);

            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", fileUri);
            AnalyzeResult result = operation.Value;

            if (result.Tables.Count < 1)
            {
                throw new Exception("No table found");
            }
            DocumentTable table = result.Tables[0];

            List<List<string>> tableContent = new();
            int rowIndex = 0;
            foreach (DocumentTableCell cell in table.Cells)
            {
                if (cell.RowIndex == 0) continue;
                if (cell.RowIndex > rowIndex)
                {
                    tableContent.Add(new List<string>());
                    rowIndex = cell.RowIndex;
                }
                if (cell.ColumnIndex == 0) continue;
                tableContent.Last().Add(cell.Content);
            }
            return tableContent;
        }
    }
}
