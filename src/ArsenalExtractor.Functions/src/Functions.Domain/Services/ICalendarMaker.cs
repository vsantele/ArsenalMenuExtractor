using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface ICalendarMaker
    {
        public string GenerateICal(IEnumerable<Menu> menus);
        public string GenerateICal(IEnumerable<Menu> menus, string favMenu);
    }
}