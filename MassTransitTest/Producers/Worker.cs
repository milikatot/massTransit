using MassTransit;
using Models;
using System.Text.Json;
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
            await Task.Delay(1000, stoppingToken);
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var scope = _serviceProvider.CreateScope();
            try
            {
                var client = scope.ServiceProvider.GetRequiredService<IRequestClient<CheckOrderStatus>>();

                var orderId = Guid.NewGuid().ToString();
                var response = await client.GetResponse<OrderStatusResult>(new { orderId });

                var responseStr = JsonSerializer.Serialize(response);
                _logger.LogInformation(responseStr);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            finally
            {
                scope.Dispose();
            }            
        }
    }
}
