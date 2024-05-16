using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using AutoMapper;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;

public class OrderService : IOrderService
{
    private readonly IOrderRepo _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepo orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ValidationException("User Id cannot be Empty");

        var orders = await _orderRepository.GetAllOrdersAsync(options, userId);

        if (orders is null) throw new ResourceNotFoundException("No orders found");

        return _mapper.Map<IEnumerable<ReadOrderDTO>>(orders);
    }

    public async Task<IEnumerable<ReadOrderDTO>> GetAllOrdersByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ValidationException("User Id cannot be Empty");

        var orders = await _orderRepository.GetAllOrdersByUserAsync(options, userId);

        if (orders is null) throw new ResourceNotFoundException("No orders found");

        return _mapper.Map<IEnumerable<ReadOrderDTO>>(orders);
    }

    public async Task<ReadOrderDTO> GetOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ValidationException("OrderId should be valid");

        var orderFound = await _orderRepository.GetOrderByIdAsync(orderId);

        if (orderFound is null) throw new ValidationException("orderId is not correct");

        return _mapper.Map<ReadOrderDTO>(orderFound);
    }

    public async Task<ReadOrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    {
        var userId = createOrderDTO.UserId;
        var addressId = createOrderDTO.AddressId;

        if (userId == Guid.Empty && addressId == Guid.Empty)
            throw new ValidationException("UserId & Address Id must be provided.");

        var order = _mapper.Map<Order>(createOrderDTO);

        var createdOrder = await _orderRepository.CreateOrderAsync(order);

        if (createdOrder == null) throw new OperationFailedException("Failed to create order.");

        return _mapper.Map<ReadOrderDTO>(createdOrder);
    }

    public async Task<bool> UpdateOrderByIdAsync(Guid orderId, UpdateOrderDTO newOrder)
    {
        if (orderId == Guid.Empty) throw new ValidationException("Order id cannot be empty");

        var oldOrder = await _orderRepository.GetOrderByIdAsync(orderId);

        if (oldOrder is null) throw new ResourceNotFoundException("Order id does not exist");

        oldOrder.Status = newOrder.Status;
        oldOrder.DateOfDelivery = DateTime.Parse(newOrder.DateOfDelivery);

        var isUpdated = await _orderRepository.UpdateOrderByIdAsync(oldOrder);

        if (!isUpdated) throw new OperationFailedException("Could not update the order");

        return isUpdated;
    }
    public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
    {
        if (orderId == Guid.Empty) throw new ValidationException("Order id cannot be empty");

        var isDeleted = await _orderRepository.DeleteOrderByIdAsync(orderId);

        if (!isDeleted) throw new OperationFailedException("Could not delete the order");

        return isDeleted;
    }
}
