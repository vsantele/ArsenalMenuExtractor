using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface IHtmlParser
    {
        public Task<WeekInfoSrc> GetWeekInfoAsync(string html);
        public string GetImageLink(string html);
    }
}