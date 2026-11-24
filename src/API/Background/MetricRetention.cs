using Zin.Infra.Repository;

public class MetricRetention(IServiceScopeFactory scopes, ILogger<MetricRetention> logger) : BackgroundService
{
    private static readonly TimeSpan Keep = TimeSpan.FromDays(7);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromHours(1));
        do
        {
            try
            {
                using IServiceScope scope = scopes.CreateScope();
                int deleted = await scope.ServiceProvider.GetRequiredService<DeviceMetricRepository>().DeleteOlderThan(DateTime.UtcNow - Keep);
                if (deleted > 0)
                {
                    logger.LogInformation("Deleted {Count} metric samples older than {Days} days", deleted, Keep.TotalDays);
                }
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Metric retention failed");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
