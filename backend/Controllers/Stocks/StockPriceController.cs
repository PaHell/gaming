using backend.Data.Repositories.Stocks;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Stocks
{
    /// <summary>
    /// Offers an endpoint to retrieve real-time stock price quotes for a given
    /// stock symbol. Fetches quote data from Finnhub’s quote API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StockPriceController(IStockPriceRepository priceRepository) : Controller
    {
        /// <summary>
        /// Get the latest stock quote for a symbol.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Quote>> GetQuote([FromQuery] string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Symbol is required.");

            var result = await priceRepository.GetQuoteAsync(symbol);
            return Ok(result);
        }
    }
}
