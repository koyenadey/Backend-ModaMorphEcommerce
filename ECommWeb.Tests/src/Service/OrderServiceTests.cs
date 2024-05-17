using Moq;
using ECommWeb.Core.src.Common;
using ECommWeb.Core.src.Entity;
using ECommWeb.Core.src.RepoAbstract;
using ECommWeb.Core.src.ValueObject;
using ECommWeb.Business.src.DTO;
using ECommWeb.Business.src.ServiceImplement.EntityServiceImplement;
using ECommWeb.Test.src.Service.Data;
using Xunit;
using AutoMapper;
using ECommWeb.Infrastructure.src;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

namespace ECommWeb.Test.src.Core;

public class OrderServiceTests
{
    private OrderService _orderService;// SUT
    //private IOrderRepo _orderRepo; // Dependencies

    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public OrderServiceTests(IMapper mapper, IPasswordService passwordService)
    {
        _mapper = mapper;
        _passwordService = passwordService;
    }

    //fake objects from MOQ
    private Mock<IOrderRepo> _orderRepoMock = new Mock<IOrderRepo>();
    private Mock<IUserRepo> _userRepoMock = new Mock<IUserRepo>();

    [Theory]
    [ClassData(typeof(OrderServiceTestData))]
    public async Task CreateOrderAsync_CreateOrder_ReturnsOrder(CreateOrderDTO createOrderDTO)
    {
        var order = _mapper.Map<Order>(createOrderDTO);
        //var productsList = createOrderDTO.OrderedProducts;

        _orderRepoMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(order);

        _orderService = new OrderService(_orderRepoMock.Object, _mapper);
        var expectedResult = await _orderService.CreateOrderAsync(createOrderDTO);

        Assert.NotNull(expectedResult);

    }

    [Fact]
    public async Task GetOrderById_ReturnsOrder()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");
        var order = new Order
        {
            UserId = userId,
            AddressId = addressId,
            Status = Status.pending,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid()
        };

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

        _orderService = new OrderService(_orderRepoMock.Object, _mapper);

        var result = await _orderService.GetOrderByIdAsync(order.Id);

        Assert.NotNull(result);

    }

    [Fact]
    public async Task UpdateOrderById_ReturnsTrue()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

        var order = new Order
        {
            UserId = userId,
            AddressId = addressId,
            Status = Status.pending,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid()
        };

        var updateOrderDTO = new UpdateOrderDTO
        {
            Status = Status.shipped,
            DateOfDelivery = DateTime.Now.ToString(),

        };
        order.Status = updateOrderDTO.Status;
        order.DateOfDelivery = DateTime.Parse(updateOrderDTO.DateOfDelivery);

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id))
                  .ReturnsAsync(order);

        _orderRepoMock.Setup(x => x.UpdateOrderByIdAsync(It.IsAny<Order>()))
                  .ReturnsAsync(true);

        _orderService = new OrderService(_orderRepoMock.Object, _mapper);

        var result = await _orderService.UpdateOrderByIdAsync(order.Id, updateOrderDTO);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteOrderById_ReturnsTrue()
    {
        Guid userId = Guid.Parse("f2404c06-a354-4f0e-8cd1-6aa91d875205");
        Guid addressId = Guid.Parse("51614cd7-b453-4398-ae98-9d5419d66307");

        var order = new Order
        {
            UserId = userId,
            AddressId = addressId,
            Status = Status.pending,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Id = Guid.NewGuid()
        };

        _orderRepoMock.Setup(x => x.GetOrderByIdAsync(order.Id)).ReturnsAsync(order);

        _orderRepoMock.Setup(x => x.DeleteOrderByIdAsync(order.Id)).ReturnsAsync(true);

        _orderService = new OrderService(_orderRepoMock.Object, _mapper);

        var result = await _orderService.DeleteOrderByIdAsync(order.Id);

        Assert.True(result);

    }

    [Fact]
    public async Task GetAllOrdersAsync_ReturnsAllOrders_BasedOnUser()
    {
        QueryOptions queryOptions = new QueryOptions();
        var user = new User
        {
            UserName = "John Miller",
            Email = "johnmiller@mail.com",
            Password = _passwordService.HashPassword("johnpassword", out var salt),
            Salt = salt,
            Role = Role.Admin
        };

        List<Order> orders = new List<Order>()
        {
            new Order
            {
                UserId = user.Id,
                AddressId = Guid.Parse("ee6e3581-7694-4f39-be74-8ce366adcf8c"),
                Status = Status.pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new Order
            {
                UserId = user.Id,
                AddressId = Guid.Parse("da8a97cf-a1c3-4ddc-8cbb-59a968dbff77"),
                Status = Status.pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new Order
            {
                UserId = user.Id,
                AddressId = Guid.Parse("c4d1ad67-5ab2-4c57-826c-0b6af2d2d394"),
                Status = Status.pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Id = Guid.NewGuid()
            }
        };

        _orderRepoMock.Setup(x => x.GetAllOrdersAsync(queryOptions, user.Id)).ReturnsAsync(orders);

        _orderService = new OrderService(_orderRepoMock.Object, _mapper);
        var ordersOfUser = await _orderService.GetAllOrdersAsync(queryOptions, user.Id);

        Assert.Equal(3, ordersOfUser.ToList().Count);
    }

}
