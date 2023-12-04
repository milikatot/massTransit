using Models;

namespace Consumers.Repositories;
internal class OrderRepository : IOrderRepository
{
    public Task<OrderStatusResult> GetAsync(string id)
    {
        var result = new OrderStatusResult()
        {
            OrderId = id,
            StatusCode = 1,
            StatusText = "pending",
            Timestamp = DateTime.UtcNow
        };

        return Task.FromResult(result);
    }
}
