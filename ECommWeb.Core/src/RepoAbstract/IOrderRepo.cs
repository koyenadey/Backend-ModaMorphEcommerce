using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Core.src.RepoAbstract;

public interface IOrderRepo
{
    public Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<Order>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId);
    public Task<Order> GetOrderByIdAsync(Guid orderId);
    public Task<Order> CreateOrderAsync(Order order, List<ProductsList> productsList);
    public Task<bool> UpdateOrderByIdAsync(Guid orderId, Order order);
    public Task<bool> DeleteOrderByIdAsync(Guid orderId);
}
