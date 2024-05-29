using ECommWeb.Business.src.DTO;
using Microsoft.AspNetCore.Mvc;
using ECommWeb.Core.src.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ECommWeb.Business.src.ServiceAbstract.EntityServiceAbstract;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;
using ECommWeb.Core.src.ValueObject;

namespace ECommWeb.Controller.src.Controller;

[ApiController]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;

    public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
    {
        _orderService = orderService;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
    }

    [Authorize] //means authenticate
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadOrderDTO>>> GetAllOrdersAsync([FromQuery] QueryOptions options)
    {
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userClaims == null) throw new InvalidOperationException("Please login to use this facility!");

        var userId = Guid.Parse(userClaims);

        var orders = await _orderService.GetAllOrdersAsync(options, userId);

        if (orders == null) return NotFound("Orders couldnot be found");
        return Ok(orders);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{id}")]
    public async Task<ActionResult<IEnumerable<ReadOrderDTO>>> GetAllOrdersByUserAsync([FromQuery] QueryOptions options, [FromRoute] Guid id)
    {
        var ordersByUser = await _orderService.GetAllOrdersByUserAsync(options, id);
        if (ordersByUser == null) return NotFound("Orders couldnot be found");
        return Ok(ordersByUser);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadOrderDTO>> GetOrderByIdAsync([FromRoute] Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null) return NotFound("Order couldnot be found");
        return Ok(order);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReadOrderDTO>> CreateOrderAsync([FromBody] CreateOrderDTO orderDto)
    {
        var order = await _orderService.CreateOrderAsync(orderDto);

        if (order == null) return BadRequest("Order couldnot be created");
        var userClaims = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        var userEmail = userClaims;
        if (userEmail != null)
            _emailService.SendEmail(userEmail, $"Order {order.Id} is {Status.processing}", "The order has been placeed successfully");
        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<ActionResult<ReadOrderDTO>> UpdateOrderByIdAsync([FromRoute] Guid id, [FromBody] UpdateOrderDTO updateOrderDTO)
    {
        var order = await _orderService.UpdateOrderByIdAsync(id, updateOrderDTO);
        if (order is null) return NotFound("Order couldnot be found");
        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteOrderByIdAsync([FromRoute] Guid id)
    {
        var deletedOrder = await _orderService.DeleteOrderByIdAsync(id);
        if (deletedOrder is null) return NotFound("Order couldnot be found");
        return Ok(deletedOrder);
    }
}
