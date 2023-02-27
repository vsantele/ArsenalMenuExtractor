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
            var regex = @"MENU DE LA SEMAINE (?:#)(?<weekNumber>\d+) \| DU (?<start>(\d{2}\/\d{2}(?:\/\d{2,4})?)) au (?<end>(\d{2}\/\d{2}(?:\/\d{2,4})?))";
            var rg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var match = rg.Match(weekTitle);
            var start = match.Groups["start"].Value.Split('/');
            var end = match.Groups["end"].Value.Split('/');
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