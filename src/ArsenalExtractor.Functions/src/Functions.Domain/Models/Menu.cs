namespace ArsenalExtractor.Functions.Domain.Models
{
    public class Menu
    {
        public string Id { get; set; } = string.Empty;
        public WeekInfo WeekInfo { get; set; } = new WeekInfo();
        public List<MenuInfo> MenuInfos { get; set; } = new List<MenuInfo>();
    }
}