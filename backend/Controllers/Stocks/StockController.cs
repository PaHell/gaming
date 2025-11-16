using AlphaVantage.Net.Stocks;
using backend.Data.Repositories.Stocks;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Stocks
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController(IStockRepository stockRepository): Controller
    {
        [HttpPost("search")]
        public async Task<ActionResult<SymbolLookup>> Search([FromQuery] string term)
        {
            return Ok(await stockRepository.SearchSymbolsAsync(term));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<StockSymbol>>> GetForExchange([FromQuery] string exchange)
        {
            return Ok(await stockRepository.GetSymbolsForExchangeAsync(exchange));
        }
    }
}
