using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DeepEyes.Functions;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vsantele.Functions.Models;

namespace Vsantele.Functions
{
    public static class GetCalendar
    {
        [FunctionName("GetCalendar")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
             [CosmosDB(
                databaseName: "arsenal",
                collectionName: "menu",
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "SELECT top 2 * FROM c order by c._ts desc")]
                IEnumerable<Menu> menus,
            ILogger log)
        {
            var calendar = new Calendar();
            foreach (var menu in menus)
            {
                var startWeek = DateTime.Parse(Utils.ConvertDate(menu.WeekInfo.DayStart, menu.WeekInfo.MonthStart, menu.WeekInfo.Year));
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


            return new OkObjectResult(serializedCalendar);
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
