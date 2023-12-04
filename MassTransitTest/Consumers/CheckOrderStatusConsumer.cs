using Consumers.Repositories;
using MassTransit;
using Models;

namespace Consumers;
public class CheckOrderStatusConsumer :
    IConsumer<CheckOrderStatus>
{
    private readonly IOrderRepository _orderRepository;

    public CheckOrderStatusConsumer(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<CheckOrderStatus> context)
    {
        var order = await _orderRepository.GetAsync(context.Message.OrderId);
        if (order == null)
            throw new InvalidOperationException("Order not found");

        await context.RespondAsync<OrderStatusResult>(new
        {
            order.OrderId,
            order.Timestamp,
            order.StatusCode,
            order.StatusText
        });
    }
}