using System.ComponentModel.Design;
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

        public CalendarMaker()
        {
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
                var startWeek = menu.WeekInfo.StartDate;
                foreach (var menuInfo in menu.MenuInfos)
                {
                    var start = menuInfo.Date.Add(new TimeSpan(12, 0, 0));
                    var end = menuInfo.Date.Add(new TimeSpan(13, 30, 0));
                    var eventItem = new CalendarEvent
                    {
                        Start = new CalDateTime(start),
                        End = new CalDateTime(end),
                        Summary = FavoriteMenu(menuInfo, favMenu),
                        Description = MakeDescription(menuInfo),
                        Location = "Arsenal"
                    };
                    calendar.Events.Add(eventItem);
                }
            }

            var serializer = new CalendarSerializer();
            var serializedCalendar = serializer.SerializeToString(calendar);

            return serializedCalendar;
        }

        private static string MakeDescription(MenuInfo menu)
        {
            var description = "";
            description += "Jour: " + menu.Day + "\\n";
            description += "Chef: " + menu.Chef + "\\n";
            description += "Végé: " + menu.Vegetarian;
            return description;
        }

        private static string FavoriteMenu(MenuInfo menu, string favMenu)
        {
            return favMenu switch
            {
                "day" => menu.Day,
                "chef" => menu.Chef,
                "vege" => menu.Vegetarian,
                _ => menu.Day
            };
        }
    }
}