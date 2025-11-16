using backend.Data.Repositories;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    /// <summary>
    /// Provides endpoints for retrieving country metadata used in company and market
    /// classifications. Data is sourced from Finnhub’s country information API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController(ICountryRepository countryRepository) : Controller
    {
        /// <summary>
        /// Get metadata for all supported countries, including country codes and names.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ICollection<CountryMetadata>>> GetAll()
        {
            var result = await countryRepository.GetAllAsync();
            return Ok(result);
        }
    }
}