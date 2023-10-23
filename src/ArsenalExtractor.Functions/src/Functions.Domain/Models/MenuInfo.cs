namespace ArsenalExtractor.Functions.Domain.Models
{
    public class MenuInfo
    {
        public string Day { get; set; } = string.Empty;
        // For data previous 2023-33
        public string Chef { get; set; } = string.Empty;
        public string Vegetarian { get; set; } = string.Empty;
        // For data after 2023-28
        public string Soup { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}