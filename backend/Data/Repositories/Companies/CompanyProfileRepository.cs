using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using backend.Libraries;
using Backend.Configuration;
using Microsoft.OpenApi;
using System.Configuration;

namespace backend.Data.Repositories.Companies
{
    public interface ICompanyProfileRepository
    {
        Task<CompanyProfile2?> GetProfile(string? symbol = null, string? isin = null, string? cusip = null);
    }

    public class CompanyProfileRepository(IHttpClientFactory httpClientFactory): ICompanyProfileRepository
    {
        private readonly FinnhubClient client = new(httpClientFactory.CreateClient(Program.FINNHUB_HTTP_CLIENT));

        public Task<CompanyProfile2?> GetProfile(string? symbol = null, string? isin = null, string? cusip = null)
        {
            return client.CompanyProfile2Async(symbol, isin, cusip);
        }
    }
}
