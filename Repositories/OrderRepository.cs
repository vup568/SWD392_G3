using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(OnlineShopContext context) : base(context) { }

    public Order? GetOrderById(int orderId)
        => _context.Orders
            .Include(o => o.User)
            .Include(o => o.Sale)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .FirstOrDefault(o => o.OrderId == orderId);

    public IList<Order> GetOrdersByUser(int userId)
        => _context.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

    public IList<Order> GetAllOrders(string? status = null, int? saleId = null)
    {
        var q = _context.Orders
            .Include(o => o.User).Include(o => o.Sale).Include(o => o.OrderItems)
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) q = q.Where(o => o.Status == status);
        if (saleId.HasValue) q = q.Where(o => o.SaleId == saleId);
        return q.OrderByDescending(o => o.OrderDate).ToList();
    }

    public bool UpdateOrderStatus(int orderId, string status, int saleId)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null) return false;
        order.Status = status;
        order.SaleId = saleId;
        _context.SaveChanges();
        return true;
    }

    public bool CancelOrder(int orderId, int userId)
    {
        var order = _context.Orders.Include(o => o.OrderItems)
            .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);
        if (order == null || order.Status != "Submitted") return false;
        order.Status = "Cancelled";
        foreach (var item in order.OrderItems)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product != null) product.Quantity += item.Quantity ?? 0;
        }
        _context.SaveChanges();
        return true;
    }

    public Order CreateOrder(int userId, List<(Product product, int quantity)> items)
    {
        var total = items.Sum(i => i.product.Price * i.quantity);
        var order = new Order
        {
            UserId      = userId,
            OrderDate   = DateTime.Now,
            TotalAmount = total,
            Status      = "Submitted"
        };
        _context.Orders.Add(order);
        _context.SaveChanges();
        foreach (var (product, qty) in items)
        {
            _context.OrderItems.Add(new OrderItem
            {
                OrderId   = order.OrderId,
                ProductId = product.ProductId,
                Quantity  = qty,
                Price     = product.Price
            });
            product.Quantity -= qty;
        }
        _context.SaveChanges();
        return order;
    }

    public bool HasFeedbackForOrder(int orderId, int userId)
        => _context.Feedbacks.Any(f => f.OrderId == orderId && f.UserId == userId);
}
