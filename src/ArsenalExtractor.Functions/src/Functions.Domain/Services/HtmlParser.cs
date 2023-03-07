using System.Text.RegularExpressions;
using ArsenalExtractor.Functions.Domain.Models;
using HtmlAgilityPack;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class HtmlParser : IHtmlParser
    {

        public HtmlParser()
        {
        }
        public string GetImageLink(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var imageLink = htmlDoc.DocumentNode.Descendants("img")
                .Where(node => node.GetAttributeValue("class", "").Contains("image-inline"))
                .First().GetAttributeValue("src", "");
            return imageLink;
        }

        public WeekInfoSrc GetWeekInfo(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var weekTitle = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "").Contains("content-core"))
                .First().Descendants("h2").First().InnerText;
            var regex = @"MENU DE LA SEMAINE (?:#)(?<weekNumber>\d+) \| DU (?<start>((\d{2}\/\d{2}(?:\/\d{2,4})?))|((\d+)((?: )(\w+))?)) au (?<end>(\d{2}\/\d{2}(?:\/\d{2,4})?)|((\d+) (\w+) (\d+)))";
            var rg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var match = rg.Match(weekTitle);
            string[] start;
            var startGroup = match.Groups["start"].Value;
            string[] end;
            var endGroup = match.Groups["end"].Value;
            if (startGroup.Contains('/'))
            {
                start = startGroup.Split("/");
                end = endGroup.Split("/");
            }
            else
            {
                var startSplit = startGroup.Split(" ");
                var endSplit = endGroup.Split(" ");
                end = new string[] { endSplit[0], endSplit[1], endSplit[2] };
                start = new string[] { startSplit[0], startSplit.Length >= 2 ? startSplit[1] : endSplit[1] };
            }
            var weekNumber = match.Groups["weekNumber"].Value;
            var dayStart = start[0];
            var monthStart = start[1];
            var dayEnd = end[0];
            var monthEnd = end[1];
            var year = start.Length == 3 ? start[2] : end.Length == 3 ? end[2] : DateTime.Now.Year.ToString();
            if (monthStart == "") monthStart = monthEnd;

            return new WeekInfoSrc
            {
                WeekNumber = weekNumber,
                DayStart = dayStart,
                MonthStart = monthStart,
                DayEnd = dayEnd,
                MonthEnd = monthEnd,
                Year = year
            };
        }
    }
}