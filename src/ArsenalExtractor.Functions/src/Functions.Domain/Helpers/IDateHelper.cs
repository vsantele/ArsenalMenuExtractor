
namespace ArsenalExtractor.Functions.Domain.Helpers
{
    public interface IDateHelper
    {
        public DateTime ConvertDate(string day, string month, string year);

        public string GetMonthNumber(string month);
    };
}