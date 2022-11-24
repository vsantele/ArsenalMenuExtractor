
namespace ArsenalExtractor.Functions.Domain.Helpers
{
    public class DateHelper : IDateHelper
    {
        public DateTime ConvertDate(string day, string month, string year)
        {
            var monthNumber = GetMonthNumber(month);
            return DateTime.Parse($"{year}-{monthNumber}-{day}");
        }

        public string GetMonthNumber(string month)
        {
            month = month.ToLower();
            return month switch
            {
                "janvier" => "01",
                "février" => "02",
                "mars" => "03",
                "avril" => "04",
                "mai" => "05",
                "juin" => "06",
                "juillet" => "07",
                "août" => "08",
                "septembre" => "09",
                "octobre" => "10",
                "novembre" => "11",
                "décembre" => "12",
                _ => month,
            };
        }
    };
}