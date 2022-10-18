using ArsenalExtractor.Functions.Options;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Extensions.Options;

namespace ArsenalExtractor.Functions.Domain.Services
{
    public class FormRecognition : IFormRecognition
    {
        private readonly DocumentAnalysisClient _client;

        public FormRecognition(IOptions<AzureFormRecognizer> options)
        {
            string endpoint = options.Value.Endpoint;
            string apiKey = options.Value.ApiKey;
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);
            _client = client;
        }

        public async Task<List<List<string>>> ExtractMenuAsync(string imageUrl)
        {
            Uri fileUri = new(imageUrl);

            AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "menuByLabel", fileUri);
            AnalyzeResult result = operation.Value;

            var menu = new List<List<string>>();

            var document = result.Documents.Where(d => d.DocumentType == "menuByLabel:menuByLabel").FirstOrDefault();
            if (document == null)
            {
                throw new Exception("No document found");
            }
            var fields = document.Fields;
            var dayMonday = fields["dayMonday"].Value.AsString();
            var dayTuesday = fields["dayTuesday"].Value.AsString();
            var dayWednesday = fields["dayWednesday"].Value.AsString();
            var dayThursday = fields["dayThursday"].Value.AsString();
            var dayFriday = fields["dayFriday"].Value.AsString();
            var chefMonday = fields["chefMonday"].Value.AsString();
            var chefTuesday = fields["chefTuesday"].Value.AsString();
            var chefWednesday = fields["chefWednesday"].Value.AsString();
            var chefThursday = fields["chefThursday"].Value.AsString();
            var chefFriday = fields["chefFriday"].Value.AsString();
            var vegeMonday = fields["vegeMonday"].Value.AsString();
            var vegeTuesday = fields["vegeTuesday"].Value.AsString();
            var vegeWednesday = fields["vegeWednesday"].Value.AsString();
            var vegeThursday = fields["vegeThursday"].Value.AsString();
            var vegeFriday = fields["vegeFriday"].Value.AsString();

            menu.Add(new List<string> { dayMonday, chefMonday, vegeMonday });
            menu.Add(new List<string> { dayTuesday, chefTuesday, vegeTuesday });
            menu.Add(new List<string> { dayWednesday, chefWednesday, vegeWednesday });
            menu.Add(new List<string> { dayThursday, chefThursday, vegeThursday });
            menu.Add(new List<string> { dayFriday, chefFriday, vegeFriday });

            return menu;
        }
    }
}