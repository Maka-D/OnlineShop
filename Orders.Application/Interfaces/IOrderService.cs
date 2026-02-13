using Orders.Application.DTOs;
using Shared.Models;

namespace Orders.Application.Interfaces;

public interface IOrderService
{
    public Task<Result<OrderCreateResponse>> PlaceOrderAsync(OrderCreateRequest request, string idempotencyKey);
    public Task<Result<bool>> CancelOrderAsync(int orderId, string userId);
    public Task<Result<bool>> ConfirmOrderAsync(int orderId, string userId);
    public Task<Result<OrderListResponse>> GetUserOrderHistoryAsync(string userId, int page, int size);
    public Task<Result<OrderDetailsResponse>> GetOrderDetailsAsync(int orderId, string userId);
}   