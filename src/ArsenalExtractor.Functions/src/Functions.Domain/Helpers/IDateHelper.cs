
namespace ArsenalExtractor.Functions.Domain.Helpers
{
    public interface IDateHelper
    {
        public DateTime ConvertDate(string day, string month, string year);

        public DateTime BeginOfWeek(DateTime date);
        public DateTime EndOfWeek(DateTime date);

        public string GetMonthNumber(string month);
    };
}