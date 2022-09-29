using ArsenalExtractor.Functions.Domain.Models;
using ArsenalExtractor.Functions.Domain.Providers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ArsenalExtractor.Functions
{
    public class GetMenu
    {
        private readonly ICalendarProvider _calendarProvider;
        public GetMenu(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }
        [Function("ExtractMenu")]
        [CosmosDBOutput(
            databaseName: "arsenal",
            collectionName: "menus",
            ConnectionStringSetting = "CosmosDbConnectionString")]
        public async Task<Menu> Run([TimerTrigger("0 0 18 * * 0")] MyInfo myTimer, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ExtractMenu");
            logger.LogInformation("Extract menu");
            var menuInfo = await _calendarProvider.ExtractCalendarAsync();
            logger.LogInformation($"Week:{menuInfo.Id}");
            return menuInfo;
        }
    }
    public class MyInfo
    {
        public MyScheduleStatus? ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
