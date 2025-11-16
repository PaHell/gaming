using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using backend.Libraries;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.Stocks
{
    public interface IStockRepository
    {
        Task<SymbolLookup> SearchSymbolsAsync(string term);
        Task<ICollection<StockSymbol>> GetSymbolsForExchangeAsync(string exchange);
    }

    public class StockRepository(IHttpClientFactory httpClientFactory, ApplicationConfiguration configuration): IStockRepository
    {
        private FinnhubClient CreateClient()
        {
            var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("X-Finnhub-Token", configuration.FinnhubApiKey);
            return new FinnhubClient(client);
        }

        public Task<SymbolLookup> SearchSymbolsAsync(string term)
        {
            return CreateClient().SymbolSearchAsync(term, string.Empty);
        }

        public Task<ICollection<StockSymbol>> GetSymbolsForExchangeAsync(string exchange)
        {
            return CreateClient().StockSymbolsAsync(
                exchange: exchange,
                mic: string.Empty,
                securityType: string.Empty,
                currency: string.Empty
            );
        }
    }
}
