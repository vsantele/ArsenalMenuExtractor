using ArsenalExtractor.Functions.Domain.Helpers;
using ArsenalExtractor.Functions.Domain.Providers;
using ArsenalExtractor.Functions.Domain.Services;
using ArsenalExtractor.Functions.Domain.Services.HttpClients;
using ArsenalExtractor.Functions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;




var host = new HostBuilder()
.ConfigureFunctionsWorkerDefaults(builder =>
{
})
.ConfigureServices(s =>
    {
        s.AddOptions<AzureFormRecognizer>().Configure<IConfiguration>((settings, configuration) => configuration.GetSection("AzureFormRecognizer").Bind(settings));
        s.AddApplicationInsightsTelemetryWorkerService();
        s.AddScoped<ICalendarProvider, CalendarProvider>();
        s.AddScoped<ICalendarMaker, CalendarMaker>();
        s.AddScoped<IHtmlParser, HtmlParser>();
        s.AddScoped<IDateHelper, DateHelper>();
        s.AddScoped<IFormRecognition, FormRecognition>();
        s.AddHttpClient<UnamurHttpClient>();
    })
.Build();
await host.RunAsync();
