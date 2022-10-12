using System.Collections.Generic;
using System.Threading.Tasks;
using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Providers
{
    public interface ICalendarProvider
    {
        public string GetCalendar(IEnumerable<Menu> menus);
        public string GetCalendar(IEnumerable<Menu> menus, string favMenu);

        public Task<Menu> ExtractCalendarAsync();
    }
}