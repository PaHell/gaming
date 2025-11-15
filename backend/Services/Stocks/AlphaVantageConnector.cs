using Ecng.Common;
using StockSharp.Messages;

namespace backend.Services.Stocks
{
    public class AlphaVantageConnector : AsyncMessageAdapter
    {
        public AlphaVantageConnector(IdGenerator transactionIdGenerator) : base(transactionIdGenerator)
        {
            HeartbeatInterval = TimeSpan.FromSeconds(5);

            // Add support for market data and transactions
            this.AddMarketDataSupport();

            // Add supported market data types
            this.AddSupportedMarketDataType(DataType.Ticks);
            this.AddSupportedMarketDataType(DataType.MarketDepth);
            this.AddSupportedMarketDataType(DataType.Level1);
            this.AddSupportedMarketDataType(DataType.CandleTimeFrame);
        }
    }
}
