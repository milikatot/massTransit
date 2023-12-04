using MassTransit;
using Models;
using System.Threading;

namespace Producers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var scope = _serviceProvider.CreateScope();
            try
            {
                var client = scope.ServiceProvider.GetRequiredService<IRequestClient<CheckOrderStatus>>();

                var orderId = Guid.NewGuid().ToString();
                var response = await client.GetResponse<OrderStatusResult>(new { orderId });

            }
            catch (Exception ex)
            {

            }
            finally
            {
                scope.Dispose();
            }


            await Task.Delay(1000, stoppingToken);
        }
    }
}
