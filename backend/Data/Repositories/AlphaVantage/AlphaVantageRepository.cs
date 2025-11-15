using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.AlphaVantage
{
    public interface IAlphaVantageRepository
    {
        Task<ICollection<SymbolSearchMatch>> SearchStockSymbols(string term);
    }

    public class AlphaVantageRepository(ApplicationConfiguration configuration): IAlphaVantageRepository
    {
        private AlphaVantageClient CreateClient()
        {
            return new AlphaVantageClient(configuration.AlphaVantage.ApiKey);
        }

        public Task<ICollection<SymbolSearchMatch>> SearchStockSymbols(string term)
        {
            var stocksClient = CreateClient().Stocks();
            return stocksClient.SearchSymbolAsync(term);
        }

        public Task<GlobalQuote?> GetStockSymbolQuote(string symbol)
        {
            var stocksClient = CreateClient().Stocks();
            return stocksClient.GetGlobalQuoteAsync(symbol);
        }
    }
}
