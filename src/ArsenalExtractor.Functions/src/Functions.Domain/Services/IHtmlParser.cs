using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface IHtmlParser
    {
        public WeekInfoSrc GetWeekInfo(string html);
        public string GetImageLink(string html);
    }
}