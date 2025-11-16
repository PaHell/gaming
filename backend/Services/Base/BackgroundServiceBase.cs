using Backend.Data.Models.General;

namespace backend.Services.Base
{
    public abstract class BackgroundServiceBase: IHostedService, IDisposable
    {
        public abstract IServiceScopeFactory ServiceScopeFactory { get; }
        public abstract string Key { get; }
        public abstract string Name { get; }
        public abstract TimeSpan RepeatEvery { get; }
        public ServiceResult? LastRunResult { get; private set; }

        private CancellationTokenSource? _cts;
        private Task? _backgroundTask;

        public BackgroundServiceBase()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var appCache = scope.ServiceProvider.GetRequiredService<AppCache>();
            appCache.BackgroundServices.Push(this);
        }

        public abstract Task<bool> DoesRunAfterStartup();

        public abstract Task<ServiceResult> DoWorkAsync();

        private async Task<ServiceResult?> SafeExecuteAsync(CancellationToken token)
        {
            try
            {
                return await DoWorkAsync();
            }
            catch (Exception ex)
            {
                // TODO: Inject ILogger and log the failure
                Console.WriteLine($"BackgroundService '{Name}' failed: {ex}");
                return null;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _backgroundTask = Task.Run(async () =>
            {
                var token = _cts.Token;

                if (await DoesRunAfterStartup())
                {
                    LastRunResult = await SafeExecuteAsync(token);
                }

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(RepeatEvery, token);
                        LastRunResult = await SafeExecuteAsync(token);
                    }
                    catch (TaskCanceledException) { }
                }

            }, _cts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_cts == null)
                return;

            _cts.Cancel();

            if (_backgroundTask != null)
            {
                await Task.WhenAny(_backgroundTask, Task.Delay(-1, cancellationToken));
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
