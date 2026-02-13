using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Interfaces;

namespace Orders.API.Controllers;

[Authorize(Roles = "User")]
public class OrdersController :BaseController
{
   private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create/order")]
    public async Task<IActionResult> PlaceOrder(
        [FromBody] OrderCreateRequest request, 
        [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey)
    {
        var userId = UserHelper.GetUserId(HttpContext.User);
        request.UserId = userId;
        var result = await _orderService.PlaceOrderAsync(request, idempotencyKey);
        return HandleResult(result);
    }

    [HttpPost("cancel-order/{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var userId = UserHelper.GetUserId(HttpContext.User);
        var result = await _orderService.CancelOrderAsync(id, userId);
        
        return HandleResult(result);
    }

    [HttpGet("get/user-orders")]
    public async Task<IActionResult> GetUserOrdersHistory([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var userId = UserHelper.GetUserId(HttpContext.User);
        var result = await _orderService.GetUserOrderHistoryAsync(userId, page, size);
        
        return HandleResult(result);
    }

    [HttpGet("get/order-details/{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var userId = UserHelper.GetUserId(HttpContext.User);
        var result = await _orderService.GetOrderDetailsAsync(id, userId);

        return HandleResult(result);
    }
    
    [HttpPost("confirm-order/{id}")]
    public async Task<IActionResult> ConfirmOrder(int id)
    {
        var userId = UserHelper.GetUserId(HttpContext.User);
        var result = await _orderService.CancelOrderAsync(id, userId);
        
        return HandleResult(result);
    }
}