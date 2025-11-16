using backend.Libraries;

namespace backend.Data.Repositories
{
    public interface ICountryRepository
    {
        Task<ICollection<CountryMetadata>> GetAllAsync();
    }

    public class CountryRepository(IHttpClientFactory httpClientFactory): ICountryRepository
    {
        private readonly FinnhubClient client = new(httpClientFactory.CreateClient(Program.FINNHUB_HTTP_CLIENT));

        public Task<ICollection<CountryMetadata>> GetAllAsync()
        {
            return client.CountryAsync();
        }
    }
}
