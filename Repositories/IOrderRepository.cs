using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Order? GetOrderById(int orderId);
    IList<Order> GetOrdersByUser(int userId);
    IList<Order> GetAllOrders(string? status = null, int? saleId = null);
    bool UpdateOrderStatus(int orderId, string status, int saleId);
    bool CancelOrder(int orderId, int userId);
    Order CreateOrder(int userId, List<(Product product, int quantity)> items);
    bool HasFeedbackForOrder(int orderId, int userId);
}
