using Models;

namespace Consumers.Repositories;
public interface IOrderRepository
{
    Task<OrderStatusResult> GetAsync(string id);
}
