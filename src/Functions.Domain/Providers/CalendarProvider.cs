using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArsenalExtractor.Functions.Domain.Models;
using ArsenalExtractor.Functions.Domain.Services;
using ArsenalExtractor.Functions.Domain.Services.HttpClients;

namespace ArsenalExtractor.Functions.Domain.Providers
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly UnamurHttpClient _client;
        private readonly ICalendarMaker _calendarMaker;
        private readonly IFormRecognition _formRecognition;
        private readonly IHtmlParser _htmlParser;

        public CalendarProvider(
            ICalendarMaker calendarMaker,
            IFormRecognition formRecognition,
            UnamurHttpClient client,
            IHtmlParser htmlParser)

        {
            _calendarMaker = calendarMaker;
            _formRecognition = formRecognition;
            _client = client;
            _htmlParser = htmlParser;
        }

        public async Task<Menu> ExtractCalendarAsync()
        {
            var html = await _client.GetArsenalMenuPageAsync();
            var weekInfo = _htmlParser.GetWeekInfo(html);
            var imageLink = _htmlParser.GetImageLink(html);
            var menuDetails = await _formRecognition.ExtractMenuAsync(imageLink);

            return new Menu
            {
                Id = weekInfo.WeekNumber,
                WeekInfo = weekInfo,
                MenuDetails = menuDetails
            };
        }

        public string GetCalendar(IEnumerable<Menu> menus)
        {
            return GetCalendar(menus, "day");
        }
        public string GetCalendar(IEnumerable<Menu> menus, string favMenu)
        {
            favMenu = ValidatedFavMenu(favMenu);
            return _calendarMaker.GenerateICal(menus, favMenu);
        }

        static private string ValidatedFavMenu(string favMenu)
        {
            favMenu = favMenu.ToLower();
            return favMenu switch
            {
                "vegetarian" or "vege" or "vegé" or "vegétarien" => "vege",
                "chef" => "chef",
                "day" or "jour" or _ => "day",
            };
        }

    }
}