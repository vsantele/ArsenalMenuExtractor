using System;
using Microsoft.Azure.WebJobs;

namespace DeepEyes.Functions
{
    public class Utils
    {
        public static string ConvertDate(string day, string month, string year)
        {
            var monthNumber = GetMonthNumber(month);
            return $"{year}-{monthNumber}-{day}";
        }

        public static string GetMonthNumber(string month)
        {
            month = month.ToLower();
            Console.WriteLine(month);
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
                _ => "01",
            };
        }
    };
}