using System.Collections.Generic;
using System.Threading.Tasks;
using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface ICalendarMaker
    {
        public string GenerateICal(IEnumerable<Menu> menus);
    }
}