using ArsenalExtractor.Functions.Options;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class FormRecognition : IFormRecognition
    {
        private readonly DocumentAnalysisClient _client;
        private readonly string _modelId;

        public FormRecognition(IOptions<AzureFormRecognizer> options)
        {
            string endpoint = options.Value.Endpoint;
            string apiKey = options.Value.ApiKey;
            _modelId = options.Value.ModelId;

            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);
            _client = client;
            _modelId = options.Value.ModelId;
        }

        public async Task<List<List<string>>> ExtractMenuAsync(string imageUrl)
        {
            Uri fileUri = new(imageUrl);

            AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, _modelId, fileUri);
            AnalyzeResult result = operation.Value;

            var menu = new List<List<string>>();

            var defaultMenu = "Pas de menu disponible";

            var document = result.Documents.Where(d => d.DocumentType == $"{_modelId}:{_modelId}").FirstOrDefault();
            if (document == null)
            {
                throw new Exception("No document found");
            }
            var fields = document.Fields;
            var dayMonday = fields["dayMonday"].Content ?? defaultMenu;
            var dayTuesday = fields["dayTuesday"].Content ?? defaultMenu;
            var dayWednesday = fields["dayWednesday"].Content ?? defaultMenu;
            var dayThursday = fields["dayThursday"].Content ?? defaultMenu;
            var dayFriday = fields["dayFriday"].Content ?? defaultMenu;
            var vegeMonday = fields["vegeMonday"].Content ?? defaultMenu;
            var vegeTuesday = fields["vegeTuesday"].Content ?? defaultMenu;
            var vegeWednesday = fields["vegeWednesday"].Content ?? defaultMenu;
            var vegeThursday = fields["vegeThursday"].Content ?? defaultMenu;
            var vegeFriday = fields["vegeFriday"].Content ?? defaultMenu;
            var soupMonday = fields["soupMonday"].Content ?? defaultMenu;
            var soupTuesday = fields["soupTuesday"].Content ?? defaultMenu;
            var soupWednesday = fields["soupWednesday"].Content ?? defaultMenu;
            var soupThursday = fields["soupThursday"].Content ?? defaultMenu;
            var soupFriday = fields["soupFriday"].Content ?? defaultMenu;

            menu.Add(new List<string> { dayMonday, vegeMonday, soupMonday });
            menu.Add(new List<string> { dayTuesday, vegeTuesday, soupTuesday });
            menu.Add(new List<string> { dayWednesday, vegeWednesday, soupWednesday });
            menu.Add(new List<string> { dayThursday, vegeThursday, soupThursday });
            menu.Add(new List<string> { dayFriday, vegeFriday, soupFriday });

            return menu;
        }
    }
}