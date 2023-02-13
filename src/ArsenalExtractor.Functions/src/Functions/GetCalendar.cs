using System.Net;
using System.Web;
using ArsenalExtractor.Functions.Domain.Models;
using ArsenalExtractor.Functions.Domain.Providers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace ArsenalExtractor.Functions
{
    public class GetCalendar
    {
        private readonly ICalendarProvider _calendarProvider;
        public GetCalendar(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }

        [Function("GetCalendar")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req,
             [CosmosDBInput(
                databaseName: "%CosmosDBDatabaseName%",
                containerName: "menus",
                Connection = "CosmosDbConnectionString",
                SqlQuery = "SELECT top 2 * FROM c order by c._ts desc")]
                IEnumerable<Menu> menus)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/calendar");

            var query = HttpUtility.ParseQueryString(req.Url.Query);
            var favMenu = query.Get("menu") ?? "";

            var calendar = _calendarProvider.GetCalendar(menus, favMenu);

            response.WriteString(calendar);

            return response;
        }
    }
}
