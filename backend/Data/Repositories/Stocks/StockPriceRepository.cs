using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using backend.Libraries;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.Stocks
{
    public interface IStockPriceRepository
    {
        Task<Quote> GetQuoteAsync(string symbol);
    }

    public class StockPriceRepository(IHttpClientFactory httpClientFactory): IStockPriceRepository
    {
        private readonly FinnhubClient client = new(httpClientFactory.CreateClient(Program.FINNHUB_HTTP_CLIENT));

        public Task<Quote> GetQuoteAsync(string symbol)
        {
            return client.QuoteAsync(symbol);
        }
    }
}
