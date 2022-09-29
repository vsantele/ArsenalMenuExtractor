using System.Collections.Generic;
namespace ArsenalExtractor.Functions.Domain.Models
{
    public class Menu
    {
        public string Id { get; set; } = string.Empty;
        public WeekInfo WeekInfo { get; set; } = new WeekInfo();
        public List<List<string>> MenuDetails { get; set; } = new List<List<string>>();
    }
}