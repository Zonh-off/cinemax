using Core.Models;

namespace Core.Interfaces;

public interface IOrderService
{
    Task<OrderSummaryModel> CreatePendingOrderAsync(string userId, CreateOrderModel req);
    Task ConfirmOrderAsync(string userId, int orderId);
}