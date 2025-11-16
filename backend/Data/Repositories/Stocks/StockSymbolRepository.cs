using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using backend.Libraries;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.Stocks
{
    public interface IStockSymbolRepository
    {
        Task<SymbolLookup> SearchSymbolsAsync(string term);
        Task<ICollection<StockSymbol>> GetSymbolsForExchangeAsync(string exchange);
    }

    public class StockSymbolRepository(IHttpClientFactory httpClientFactory): IStockSymbolRepository
    {
        private readonly FinnhubClient client = new(httpClientFactory.CreateClient(Program.FINNHUB_HTTP_CLIENT));

        public Task<SymbolLookup> SearchSymbolsAsync(string term)
        {
            return client.SymbolSearchAsync(term, string.Empty);
        }

        public Task<ICollection<StockSymbol>> GetSymbolsForExchangeAsync(string exchange)
        {
            return client.StockSymbolsAsync(
                exchange: exchange,
                mic: string.Empty,
                securityType: string.Empty,
                currency: string.Empty
            );
        }
    }
}
