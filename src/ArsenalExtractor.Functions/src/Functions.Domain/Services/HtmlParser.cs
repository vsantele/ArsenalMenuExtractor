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