using backend.Data.Repositories.Companies;
using backend.Libraries;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Companies
{
    /// <summary>
    /// Provides access to company profile data using various identifiers such as
    /// symbol, ISIN, or CUSIP. Retrieves detailed profile information from
    /// Finnhub’s Company Profile 2 API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyProfileController(ICompanyProfileRepository profileRepository) : Controller
    {
        /// <summary>
        /// Get company profile data by symbol, ISIN, or CUSIP.
        /// At least one identifier is required.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<CompanyProfile2?>> GetProfile(
            [FromQuery] string? symbol = null,
            [FromQuery] string? isin = null,
            [FromQuery] string? cusip = null)
        {
            if (symbol is null && isin is null && cusip is null)
                return BadRequest($"At least one of '{nameof(symbol)}', '{nameof(isin)}', or '{nameof(cusip)}' must be provided.");

            var result = await profileRepository.GetProfile(symbol, isin, cusip);

            // Finnhub returns null if the profile isn't found
            if (result is null)
                return NotFound("Company profile not found.");

            return Ok(result);
        }
    }
}
