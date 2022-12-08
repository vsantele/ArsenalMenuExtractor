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

            var defaultMenu = "Pas de menu disponible";

            var document = result.Documents.Where(d => d.DocumentType == "menuByLabel:menuByLabel").FirstOrDefault();
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
            var chefMonday = fields["chefMonday"].Content ?? defaultMenu;
            var chefTuesday = fields["chefTuesday"].Content ?? defaultMenu;
            var chefWednesday = fields["chefWednesday"].Content ?? defaultMenu;
            var chefThursday = fields["chefThursday"].Content ?? defaultMenu;
            var chefFriday = fields["chefFriday"].Content ?? defaultMenu;
            var vegeMonday = fields["vegeMonday"].Content ?? defaultMenu;
            var vegeTuesday = fields["vegeTuesday"].Content ?? defaultMenu;
            var vegeWednesday = fields["vegeWednesday"].Content ?? defaultMenu;
            var vegeThursday = fields["vegeThursday"].Content ?? defaultMenu;
            var vegeFriday = fields["vegeFriday"].Content ?? defaultMenu;

            menu.Add(new List<string> { dayMonday, chefMonday, vegeMonday });
            menu.Add(new List<string> { dayTuesday, chefTuesday, vegeTuesday });
            menu.Add(new List<string> { dayWednesday, chefWednesday, vegeWednesday });
            menu.Add(new List<string> { dayThursday, chefThursday, vegeThursday });
            menu.Add(new List<string> { dayFriday, chefFriday, vegeFriday });

            return menu;
        }
    }
}