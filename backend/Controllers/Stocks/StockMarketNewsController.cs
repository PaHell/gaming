using backend.Data.Repositories.Stocks;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Stocks
{
    /// <summary>
    /// Exposes endpoints for retrieving broad market news categories as well as
    /// company-specific news within a given date range. Powered by Finnhub’s
    /// market news and company news APIs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StockMarketNewsController(IStockMarketNewsRepository newsRepository) : Controller
    {
        /// <summary>
        /// Get general or category-specific market news.
        /// </summary>
        [HttpGet("market")]
        public async Task<ActionResult<ICollection<MarketNews>>> GetMarketNews(
            [FromQuery] string category = "general",
            [FromQuery] int afterId = 0)
        {
            var result = await newsRepository.GetMarketNews(category, afterId);
            return Ok(result);
        }

        /// <summary>
        /// Get company-specific news within a date range.
        /// </summary>
        [HttpGet("company")]
        public async Task<ActionResult<ICollection<CompanyNews>>> GetCompanyNews(
            [FromQuery] string symbol,
            [FromQuery] DateTimeOffset from,
            [FromQuery] DateTimeOffset to)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Symbol is required.");

            var result = await newsRepository.GetCompanyNews(symbol, from, to);
            return Ok(result);
        }
    }
}
