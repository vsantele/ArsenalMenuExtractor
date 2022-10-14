
namespace ArsenalExtractor.Functions.Domain.Helpers
{
    public interface IDateHelper
    {
        public string ConvertDate(string day, string month, string year);

        public string GetMonthNumber(string month);
    };
}