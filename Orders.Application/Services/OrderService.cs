using Orders.Application.DTOs;
using Orders.Application.ExternalServices;
using Orders.Application.Interfaces;
using Orders.Domain.Interfaces;
using Shared.Models;

namespace Orders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductCatalogClient _productCatalogClient;

    public OrderService(IOrdersRepository orderRepository, IUnitOfWork unitOfWork,
        IProductCatalogClient productCatalogClient)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productCatalogClient = productCatalogClient;
    }

    public async Task<Result<OrderCreateResponse>> PlaceOrderAsync(OrderCreateRequest request, string idempotencyKey)
    {
        if (!Guid.TryParse(idempotencyKey, out var guidKey))
        {
            return Result<OrderCreateResponse>.Failure("Invalid idempotencyKey.");
        }

        var checkIdempotencyKey = await _orderRepository.ExistsByIdempotencyKeyAsync(guidKey);
        if (checkIdempotencyKey)
        {
            return Result<OrderCreateResponse>.Failure("Order already exists.");
        }

        var productIds = request.Items.Select(i => i.ProductId);

        var products = await _productCatalogClient.GetByIdsAsync(productIds);
        var productDict = products.Products.ToDictionary(p => p.Id, p => p);

        var order = new Domain.Models.Order(request.UserId);
        order.IdempotencyKey = guidKey;

        foreach (var item in request.Items)
        {
            if (!productDict.TryGetValue(item.ProductId, out var product))
            {
                return Result<OrderCreateResponse>.Failure($"Product {item.ProductId} not found.");
            }

            if (!product.IsActive)
            {
                return Result<OrderCreateResponse>.Failure($"Product {product.Name} is inactive.");
            }

            if (product.StockQuantity < item.Quantity)
            {
                return Result<OrderCreateResponse>.Failure($"Not enough product stock. Requested: {item.Quantity}, in stock: {product.StockQuantity}");
            }
            
            order.AddItem(product.Id, product.Name, product.Price, item.Quantity);
        }

        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync();

        return Result<OrderCreateResponse>.Success(new OrderCreateResponse()
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice
        });
    }

    public async Task<Result<bool>> CancelOrderAsync(int orderId, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return Result<bool>.Failure("Order not found.");

        if (order.UserId != userId)
            return Result<bool>.Failure("Unauthorized user.You do not have permission to cancel this order!");

        try
        {
            order.Cancel();
        }
        catch (InvalidOperationException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }

        await _unitOfWork.SaveChangesAsync();

        var itemsToReleaseStock = order.Items.Select(i => new StockUpdate()
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        });
        var stockResult = await _productCatalogClient.ReleaseStockAsync(itemsToReleaseStock);
        if (!stockResult)
        {
            //to avoid such cases outbox pattern might be used, to save failed processes 
            //and have background job retry them
            //currently I'm leaving this as just failure result but wouldn't do in prod
            return Result<bool>.Failure("Order cancelled, but stock update failed.");
        }

        return Result<bool>.Success();
    }
    
    public async Task<Result<bool>> ConfirmOrderAsync(int orderId, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return Result<bool>.Failure("Order not found.");

        if (order.UserId != userId)
            return Result<bool>.Failure("Unauthorized user.You do not have permission to confirm this order!");

        try
        {
            order.Confirm();
        }
        catch (InvalidOperationException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }

        await _unitOfWork.SaveChangesAsync();

        var itemsToDecreaseStock = order.Items.Select(i => new StockUpdate()
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        });
        var stockResult = await _productCatalogClient.DecreaseStockAsync(itemsToDecreaseStock);
        if (!stockResult)
        {
            //similar case to cancel flow
            return Result<bool>.Failure("Order Confirmed, but stock update failed.");
        }

        return Result<bool>.Success();
    }

    public async Task<Result<OrderListResponse>> GetUserOrderHistoryAsync(string userId, int page, int size)
    {
        var orders = await _orderRepository.GetUserOrdersAsync(userId, page, size);

        var orderDtos = orders.Select(o => new Order
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            Status = o.Status.ToString(),
            UserId = o.UserId,
            Items = o.Items.Select(i => new OrderItem()
            {
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            })
        }).ToList();

        return Result<OrderListResponse>.Success(new OrderListResponse() { Orders = orderDtos });
    }

    public async Task<Result<OrderDetailsResponse>> GetOrderDetailsAsync(int orderId, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
            return Result<OrderDetailsResponse>.Failure("Order not found.");

        if (order.UserId != userId)
            return Result<OrderDetailsResponse>.Failure("You do not have permission to view this order.");

        var response = new OrderDetailsResponse
        {
            Order = new Order()
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status.ToString(),
                UserId = order.UserId,
                Items = order.Items.Select(i => new OrderItem()
                {
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })
            }
        };

        return Result<OrderDetailsResponse>.Success(response);
    }
}