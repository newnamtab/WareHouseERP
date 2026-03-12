namespace WareHouseERP.Application.UseCases;

using DTOs;
using WareHouseERP.Domain.Entities;
using WareHouseERP.Domain.Repositories;

public class CreateOrderUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> ExecuteAsync(string userId, CreateOrderRequest request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId is required", nameof(userId));
        if (request?.OrderLines == null || !request.OrderLines.Any())
            throw new InvalidOperationException("Order must contain at least one line item");

        return await _unitOfWork.ExecuteTransactionAsync(async () =>
        {
            var order = new Order(userId, request.OrderNumber);

            // Process each order line and reserve stock
            foreach (var lineRequest in request.OrderLines)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(lineRequest.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product {lineRequest.ProductId} not found");

                // Reserve stock
                if (!product.ReserveStock(lineRequest.Quantity))
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {product.Name}. Available: {product.AvailableStock}, Requested: {lineRequest.Quantity}");

                // Create order line
                var orderLine = new OrderLine(
                    product.Id,
                    product.Name,
                    product.Sku,
                    product.Price,
                    lineRequest.Quantity);

                order.AddOrderLine(orderLine);

                // Update product with reserved stock
                await _unitOfWork.ProductRepository.UpdateAsync(product);
            }

            order.ConfirmOrder();
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(order);
        });
    }

    private OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            OrderLines = order.OrderLines.Select(ol => new OrderLineDto
            {
                Id = ol.Id,
                ProductName = ol.ProductName,
                ProductSku = ol.ProductSku,
                UnitPrice = ol.UnitPrice,
                Quantity = ol.Quantity,
                LineTotal = ol.LineTotal
            }).ToList()
        };
    }
}