using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using ArsenalExtractor.Functions.Domain.Models;
using HtmlAgilityPack;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class HtmlParser(IOpenAiService openAiService) : IHtmlParser
    {
        public string GetImageLink(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var imageNode = htmlDoc.DocumentNode.Descendants("img").Where(node => node.GetAttributeValue("src", "").Contains("rs-menus")).First() ?? throw new Exception("Image not found");
            var imageLink = imageNode.GetAttributeValue("src", "") ?? throw new Exception("Image link not found");

            return imageLink;
        }

        async public Task<WeekInfoSrc> GetWeekInfoAsync(string html)
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            var weekTitle = htmlDoc.DocumentNode.Descendants("div").Where(div => div.GetClasses().Contains("paragraph--type--image")).First()
                .Descendants("h2").First().InnerText;

            var response = await openAiService.SendQuery(CreateOpenAiQuery(weekTitle));
            Console.WriteLine(response);
            using JsonDocument structuredJson = JsonDocument.Parse(response);
            var startDateStr = structuredJson.RootElement.GetProperty("startDate").GetString() ?? throw new Exception("Start date not found");
            DateOnly startDate = DateOnly.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var weekNumber = GetWeekNumber(startDate);
            return new WeekInfoSrc
            {
                WeekNumber = weekNumber.ToString().PadLeft(2, '0'),
                DayStart = startDate.Day.ToString(),
                MonthStart = startDate.Month.ToString().PadLeft(2, '0'),
                DayEnd = startDate.AddDays(4).Day.ToString(),
                MonthEnd = startDate.AddDays(4).Month.ToString().PadLeft(2, '0'),
                Year = startDate.Year.ToString()
            };
        }

        private static DateTime FirstDateOfWeek(int year, int weekOfYear, DayOfWeek startOfWeek)
        {
            DateTime jan1 = new(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int daysOffset = (int)startOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, startOfWeek);
            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        private static int GetWeekNumber(DateOnly date)
        {
            var cal = CultureInfo.CurrentCulture.Calendar;
            return cal.GetWeekOfYear(date.ToDateTime(new TimeOnly(12, 0)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private static List<IOpenAiService.Message> CreateOpenAiQuery(string weekTitle)
        {
            var messages = new List<IOpenAiService.Message>
            {
                new() { Type = IOpenAiService.ChatMessageType.System, Content = GetSystemMessage() },
                new() { Type = IOpenAiService.ChatMessageType.User, Content = weekTitle }
            };
            return messages;
        }
        private static string GetSystemMessage()
        {
            return @"
Tu es un assistant qui aide l'utilisateur à transformer une semaine en langage naturel vers le format JSON.
Le schema ressemble à :
```
{
    ""startDate"": ""2024-12-25"",
    ""endDate"": ""2024-12-31""
}
```
La date doit être sous le format yyyy-MM-dd.

Pour t'aider voici la date du Jour: " + DateTime.Now.ToString("yyyy-MM-dd");

        }
    }
}