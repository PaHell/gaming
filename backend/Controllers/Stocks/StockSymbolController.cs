using backend.Data.Repositories.Stocks;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Stocks
{
    /// <summary>
    /// Provides endpoints for searching stock symbols and retrieving available
    /// symbols for a specific stock exchange. Uses Finnhub data sources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StockSymbolController(IStockSymbolRepository stockSymbolRepository) : Controller
    {
        /// <summary>
        /// Search for stock symbols by name or keyword.
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<SymbolLookup>> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required.");

            var result = await stockSymbolRepository.SearchSymbolsAsync(term);
            return Ok(result);
        }

        /// <summary>
        /// Get all stock symbols for a given stock exchange.
        /// </summary>
        [HttpGet("exchange")]
        public async Task<ActionResult<ICollection<StockSymbol>>> GetSymbolsForExchange(
            [FromQuery] string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest("Exchange code is required.");

            var result = await stockSymbolRepository.GetSymbolsForExchangeAsync(value);
            return Ok(result);
        }
    }
}
