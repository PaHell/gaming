using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using backend.Libraries;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.Stocks
{
    public interface IStockMarketNewsRepository
    {
        Task<ICollection<MarketNews>> GetMarketNews(string category = "general", int afterId = 0);
        Task<ICollection<CompanyNews>> GetCompanyNews(string symbol, DateTimeOffset from, DateTimeOffset to);
    }

    public class StockMarketNewsRepository(IHttpClientFactory httpClientFactory): IStockMarketNewsRepository
    {
        private readonly FinnhubClient client = new(httpClientFactory.CreateClient(Program.FINNHUB_HTTP_CLIENT));
        
        public Task<ICollection<MarketNews>> GetMarketNews(string category = "general", int afterId = 0)
        {
            return client.MarketNewsAsync(category, afterId);
        }

        public Task<ICollection<CompanyNews>> GetCompanyNews(string symbol, DateTimeOffset from, DateTimeOffset to)
        {
            return client.CompanyNewsAsync(symbol, from, to);
        }
    }
}
