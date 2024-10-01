namespace ArsenalExtractor.Functions.Domain.Services.HttpClients
{
    public class UnamurHttpClient
    {
        private readonly HttpClient _client;

        public UnamurHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://www.unamur.be/");
            _client = client;
        }

        public async Task<string> GetArsenalMenuPageAsync()
        {
            var response = await _client.GetAsync("fr/campus/vivre/restaurant");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}