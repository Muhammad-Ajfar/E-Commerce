using E_Commerce.DTOs;
using E_Commerce.Helpers;

namespace E_Commerce.Services.OrderServices
{
    public interface IOrderService
    {
        Task<ApiResponses<string>> CreateOrder(OrderCreateDTO orderCreateDTO, Guid userId);
        Task<ApiResponses<ICollection<OrderGetDTO>>> GetOrders(Guid userId);
        Task<ApiResponses<string>> UpdateOrderStatus(Guid orderId, string status);
        Task<ApiResponses<string>> DeleteOrder(Guid orderId);
        Task<ApiResponses<ICollection<OrderGetDTO>>> GetAllOrders();
        Task<ApiResponses<ICollection<OrderGetDTO>>> GetOrdersByUser(Guid userId);
        Task<ApiResponses<decimal>> GetTotalRevenue();
        Task<ApiResponses<int>> GetTotalProductsPurchased();
    }

}
