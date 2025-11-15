using AlphaVantage.Net.Stocks;
using backend.Data.Repositories.AlphaVantage;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.AlphaVantage
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockSymbolController(IAlphaVantageRepository alphaVantageRepository): Controller
    {
        [HttpPost("search")]
        public async Task<ActionResult<ICollection<SymbolSearchMatch>>> Search([FromQuery] string term)
        {
            return Ok(await alphaVantageRepository.SearchStockSymbols(term));
        }
    }
}
