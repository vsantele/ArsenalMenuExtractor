using System.Collections.Generic;
using System.Threading.Tasks;
using ArsenalExtractor.Functions.Domain.Models;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface IFormRecognition
    {
        public Task<List<List<string>>> ExtractMenuAsync(string imageUrl);
    }
}