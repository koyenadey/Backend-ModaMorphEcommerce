using System.Collections;
using Microsoft.EntityFrameworkCore;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Infrastructure.src.Database;

namespace ECommWeb.Infrastructure.src.Repo;

public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _context;

    public OrderRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var productsList = order.OrderedProducts;

                foreach (var product in productsList)
                {
                    product.OrderId = order.Id;
                }

                await _context.Orders.AddAsync(order);

                await _context.OrderedProducts.AddRangeAsync(productsList);

                foreach (var orderedProduct in productsList)
                {
                    var product = await _context.Products.FindAsync(orderedProduct.ProductId);
                    if (product != null)
                    {
                        // Deduct ordered quantity from product stock
                        product.Inventory -= orderedProduct.Quantity;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Product with ID {orderedProduct.ProductId} not found.");
                    }
                }
                await _context.SaveChangesAsync();

                var addedOrder = await _context.Orders
                                                .Include(o => o.Address)
                                                .Include(o => o.User)
                                                .FirstOrDefaultAsync(o => o.Id == order.Id);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            Console.WriteLine($"Order with order id {order.Id} is added in the address {order.AddressId}");
            return order;
        }

    }

    public async Task<Order> DeleteOrderByIdAsync(Guid orderId)
    {
        Order order = await GetOrderByIdAsync(orderId);
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        var pgNo = options.PageNo; //1
        var pgSize = options.PageSize; // 10

        if (userId == Guid.Empty) throw new ArgumentNullException("UserId cannot be empty");

        var userRole = _context.Users.FirstOrDefault(u => u.Id == userId)!.Role;

        IQueryable<Order> query = _context.Orders;

        if (userRole == Role.Admin)
        {
            var orders = query
                            .OrderByDescending(o => o.OrderDate)
                            .Include(o => o.User)
                            .Include(o => o.Address)
                            .Include(o => o.OrderedProducts)
                                .ThenInclude(op => op.Product)
                            .Skip((pgNo - 1) * pgSize)
                            .Take(pgSize);

            return await orders.ToListAsync();
        }
        else
        {
            var orders = _context.Orders.Where(o => o.UserId == userId)
                                    .OrderByDescending(o => o.OrderDate)
                                    .Include(o => o.User)
                                        .Include(o => o.Address)
                                        .Include(o => o.OrderedProducts)
                                            .ThenInclude(op => op.Product)
                                    .Skip((pgNo - 1) * pgSize)
                                    .Take(pgSize);

            return await orders.ToListAsync();
        }


    }

    public async Task<IEnumerable<Order>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId)
    {
        return await GetAllOrdersAsync(options, userId);
    }

    public async Task<Order> GetOrderByIdAsync(Guid orderId)
    {

        return await _context.Orders
                             .Include(o => o.Address)
                             .Include(o => o.User)
                             .Include(o => o.OrderedProducts)
                                .ThenInclude(op => op.Product)
                             .FirstAsync(o => o.Id == orderId);
    }

    public async Task<Order> UpdateOrderByIdAsync(Order order)
    {
        var orderFound = await GetOrderByIdAsync(order.Id);
        if (order != null && orderFound != null)
        {
            orderFound.Status = order.Status;
            orderFound.DateOfDelivery = order.DateOfDelivery;
        }
        await _context.SaveChangesAsync();

        return order;
    }
}
