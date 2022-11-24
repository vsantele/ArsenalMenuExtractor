using ArsenalExtractor.Functions.Domain.Helpers;
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
        private readonly IDateHelper _dateHelper;

        public CalendarProvider(
            ICalendarMaker calendarMaker,
            IFormRecognition formRecognition,
            UnamurHttpClient client,
            IHtmlParser htmlParser,
            IDateHelper dateHelper)

        {
            _calendarMaker = calendarMaker;
            _formRecognition = formRecognition;
            _client = client;
            _htmlParser = htmlParser;
            _dateHelper = dateHelper;
        }

        public async Task<Menu> ExtractCalendarAsync()
        {
            var html = await _client.GetArsenalMenuPageAsync();
            var weekInfoSrc = _htmlParser.GetWeekInfo(html);
            var imageLink = _htmlParser.GetImageLink(html);
            var menuDetails = await _formRecognition.ExtractMenuAsync(imageLink);

            var weekInfo = new WeekInfo
            {
                StartDate = _dateHelper.ConvertDate(weekInfoSrc.DayStart, weekInfoSrc.MonthStart, weekInfoSrc.Year),
                EndDate = _dateHelper.ConvertDate(weekInfoSrc.DayEnd, weekInfoSrc.MonthEnd, weekInfoSrc.Year)
            };

            var menuInfos = new List<MenuInfo>();
            for (var i = 0; i < menuDetails.Count; i++)
            {
                menuInfos.Add(ConvertedMenuInfo(menuDetails[i], weekInfo.StartDate, i));
            }

            return new Menu
            {
                Id = weekInfoSrc.WeekNumber,
                WeekInfo = weekInfo,
                MenuInfos = menuInfos
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

        private static MenuInfo ConvertedMenuInfo(List<string> menu, DateTime startDate, int indexDay)
        {
            return new MenuInfo
            {
                Date = startDate.AddDays(indexDay),
                Day = menu[0],
                Chef = menu[1],
                Vegetarian = menu[2]
            };
        }

    }
}