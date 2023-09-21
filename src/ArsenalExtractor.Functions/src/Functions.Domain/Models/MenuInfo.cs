namespace ArsenalExtractor.Functions.Domain.Models
{
    public class MenuInfo
    {
        public string Day { get; set; } = string.Empty;
        public string Chef { get; set; } = string.Empty;
        public string Vegetarian { get; set; } = string.Empty;
        public string Soup {get; set;} = string.Empty;
        public DateTime Date { get; set; }
    }
}