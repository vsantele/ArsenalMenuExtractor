using ArsenalExtractor.Functions.Domain.Helpers;
using ArsenalExtractor.Functions.Domain.Models;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class CalendarMaker : ICalendarMaker
    {
        private readonly IDateHelper _dateHelper;

        public CalendarMaker(IDateHelper dateHelper)
        {
            _dateHelper = dateHelper;
        }

        public string GenerateICal(IEnumerable<Menu> menus)
        {
            return GenerateICal(menus, "day");
        }
        public string GenerateICal(IEnumerable<Menu> menus, string favMenu = "day")
        {
            var calendar = new Calendar();
            calendar.AddTimeZone(new VTimeZone("Europe/Brussels"));
            foreach (var menu in menus)
            {
                var startWeek = DateTime.Parse(_dateHelper.ConvertDate(menu.WeekInfo.DayStart, menu.WeekInfo.MonthStart, menu.WeekInfo.Year));
                for (int i = 0; i < menu.MenuDetails.Count; i++)
                {
                    var start = startWeek.AddDays(i).AddHours(12);
                    var end = start.AddHours(1).AddMinutes(30);
                    var eventItem = new CalendarEvent
                    {
                        Start = new CalDateTime(start),
                        End = new CalDateTime(end),
                        Summary = menu.MenuDetails[i][MenuIndex(favMenu)],
                        Description = MakeDescription(menu.MenuDetails[i]),
                        Location = "Arsenal"
                    };
                    calendar.Events.Add(eventItem);
                }
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            return serializedCalendar;
        }

        private static string MakeDescription(List<string> menu)
        {
            var description = "";
            description += "Jour: " + menu[0] + "\\n";
            description += "Chef: " + menu[1] + "\\n";
            description += "Végé: " + menu[2];
            return description;
        }

        private static int MenuIndex(string menu)
        {
            return menu switch
            {
                "day" => 0,
                "chef" => 1,
                "vege" => 2,
                _ => 0,
            };
        }
    }
}