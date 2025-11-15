using AlphaVantage;
using Backend.Configuration;
using Ecng.Collections;
using Ecng.Logging;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using System.Security;
using AlphaVantage.Net.Common.Intervals;
using AlphaVantage.Net.Common.Size;
using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;

namespace backend.Services.Stocks
{
    public class AlphaVantageService(ApplicationConfiguration configuration)
    {
        public async Task Connect()
        {
            using var client = new AlphaVantageClient(configuration.AlphaVantage.ApiKey);
            using var stocksClient = client.Stocks();

            //StockTimeSeries stockTs = await stocksClient.GetTimeSeriesAsync("AAPL", Interval.Daily, OutputSize.Compact, isAdjusted: true);

            GlobalQuote? globalQuote = await stocksClient.GetGlobalQuoteAsync("AAPL");

            ICollection<SymbolSearchMatch> searchMatches = await stocksClient.SearchSymbolAsync("BA");

            Console.WriteLine(searchMatches);
        }
    }
}
