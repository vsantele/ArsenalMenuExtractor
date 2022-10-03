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
            // get azure form recognizer endpoint from local config
            string endpoint = options.Value.Endpoint;
            string apiKey = options.Value.ApiKey;
            Console.WriteLine($"endpoint: {endpoint}");
            Console.WriteLine($"apiKey: {apiKey}");
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);
            _client = client;
        }

        public async Task<List<List<string>>> ExtractMenuAsync(string imageUrl)
        {
            Uri fileUri = new(imageUrl);

            AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", fileUri);
            AnalyzeResult result = operation.Value;

            if (result.Tables.Count < 1)
            {
                throw new Exception("No table found");
            }
            DocumentTable table = result.Tables[0];

            List<List<string>> tableContent = new();
            int rowIndex = 0;
            foreach (DocumentTableCell cell in table.Cells)
            {
                if (cell.RowIndex == 0 || IsHeaderRow(cell.Content)) continue;
                if (cell.RowIndex > rowIndex)
                {
                    tableContent.Add(new List<string>());
                    rowIndex = cell.RowIndex;
                }
                if (cell.ColumnIndex == 0) continue;
                tableContent.Last().Add(cell.Content);
            }
            return tableContent;
        }

        private static bool IsHeaderRow(string cellContent)
        {
            cellContent = cellContent.ToLower();
            return cellContent.Contains("menu") || cellContent.Contains("du jour") || cellContent.Contains("du chef") || cellContent.Contains("végé");
        }
    }
}