using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Net.Http;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (response == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException($"Error {responseDictionary["error"]}");
                }

                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (response == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException($"Error {responseDictionary["error"]}");
                }

                return responseDictionary;
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                if (response == null || response.Contains("error"))
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                var responseListOfDictionaries = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response) ?? new();

                if (responseListOfDictionaries != null && responseListOfDictionaries.Any())
                {
                    if (responseListOfDictionaries[0].ContainsKey("error"))
                    {
                        throw new InvalidOperationException($"Error {responseListOfDictionaries[0]["error"]}");
                    }
                }

                return responseListOfDictionaries;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get,
                };

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(stream);

                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (response == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException($"Error {responseDictionary["error"]}");
                }

                return responseDictionary;
            }
        }
    }
}