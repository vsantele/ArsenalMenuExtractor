namespace ArsenalExtractor.Functions.Options
{
    public class AzureFormRecognizer
    {
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
    };

    public class AzureOpenAI
    {
        public string Endpoint { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}