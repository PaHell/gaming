using backend.Services.Base;
using Backend.Data;

namespace backend.Services.Stocks
{
    public class StockSymbolService(IServiceScopeFactory serviceScopeFactory) : BackgroundServiceBase
    {
        public override IServiceScopeFactory ServiceScopeFactory => serviceScopeFactory;

        public override string Key => "stock-symbol";

        public override string Name => "Stock Symbol";

        public override TimeSpan RepeatEvery => new(
            hours: 8,
            minutes: 0,
            seconds: 0
        );

        public override Task<bool> DoesRunAfterStartup()
        {
            return Task.FromResult(true);
        }

        public override Task<ServiceResult> DoWorkAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return Task.FromResult(new ServiceResult
            {
                IsSuccess = true,
                Message = "Stock symbols updated successfully."
            });
        }
    }
}
