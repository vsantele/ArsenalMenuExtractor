namespace ArsenalExtractor.Functions.Domain.Services
{
    public interface IFormRecognition
    {
        public Task<List<List<string>>> ExtractMenuAsync(string imageUrl);
    }
}