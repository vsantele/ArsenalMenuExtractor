using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArsenalExtractor.Functions.Domain.Helpers;
using ArsenalExtractor.Functions.Domain.Models;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
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
            var calendar = new Calendar();
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
                        Summary = menu.MenuDetails[i][0],
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
            description += "Jour: " + menu[0];
            description += "Chef: " + menu[1];
            description += "Végé: " + menu[2];
            return description;
        }
    }
}