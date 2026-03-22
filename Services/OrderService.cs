using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IOrderService
{
    // Cart
    Dictionary<int, int> GetCart(ISession session);
    void AddToCart(ISession session, int productId, int quantity = 1);
    void UpdateCart(ISession session, int productId, int quantity);
    void RemoveFromCart(ISession session, int productId);
    void ClearCart(ISession session);
    List<(Product product, int quantity)> GetCartItems(ISession session);

    // Orders
    Order CreateOrder(int userId, ISession session);
    IList<Order> GetOrdersForUser(int userId);
    Order? GetOrderById(int orderId);
    bool CancelOrder(int orderId, int userId);
    bool UpdateOrderStatus(int orderId, string status, int saleId);
    IList<Order> GetAllOrders(string? status = null, int? saleId = null);

    // Reviews
    bool SubmitReview(int orderId, int userId, int productId, string content, int rating);
    bool HasFeedbackForOrder(int orderId, int userId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository    _orderRepo;
    private readonly IProductRepository  _productRepo;
    private readonly IFeedbackRepository _feedbackRepo;

    public OrderService(
        IOrderRepository    orderRepo,
        IProductRepository  productRepo,
        IFeedbackRepository feedbackRepo)
    {
        _orderRepo    = orderRepo;
        _productRepo  = productRepo;
        _feedbackRepo = feedbackRepo;
    }

    // ── Cart (session-based) ────────────────────────────────────────────────
    public Dictionary<int, int> GetCart(ISession session)
        => session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();

    public void AddToCart(ISession session, int productId, int quantity = 1)
    {
        var cart = GetCart(session);
        cart[productId] = cart.ContainsKey(productId) ? cart[productId] + quantity : quantity;
        session.SetObjectAsJson("Cart", cart);
    }

    public void UpdateCart(ISession session, int productId, int quantity)
    {
        var cart = GetCart(session);
        if (quantity <= 0) cart.Remove(productId);
        else cart[productId] = quantity;
        session.SetObjectAsJson("Cart", cart);
    }

    public void RemoveFromCart(ISession session, int productId)
    {
        var cart = GetCart(session);
        cart.Remove(productId);
        session.SetObjectAsJson("Cart", cart);
    }

    public void ClearCart(ISession session) => session.Remove("Cart");

    public List<(Product product, int quantity)> GetCartItems(ISession session)
    {
        var cart = GetCart(session);
        if (!cart.Any()) return new();
        var ids      = cart.Keys.ToList();
        var products = _productRepo.GetProducts(status: true)
                                   .Where(p => ids.Contains(p.ProductId))
                                   .ToList();
        return products.Select(p => (p, cart[p.ProductId])).ToList();
    }

    // ── Orders ──────────────────────────────────────────────────────────────
    public Order CreateOrder(int userId, ISession session)
    {
        var items = GetCartItems(session);
        var order = _orderRepo.CreateOrder(userId, items);
        ClearCart(session);
        return order;
    }

    public IList<Order> GetOrdersForUser(int userId) => _orderRepo.GetOrdersByUser(userId);

    public Order? GetOrderById(int orderId) => _orderRepo.GetOrderById(orderId);

    public bool CancelOrder(int orderId, int userId) => _orderRepo.CancelOrder(orderId, userId);

    public bool UpdateOrderStatus(int orderId, string status, int saleId)
        => _orderRepo.UpdateOrderStatus(orderId, status, saleId);

    public IList<Order> GetAllOrders(string? status = null, int? saleId = null)
        => _orderRepo.GetAllOrders(status, saleId);

    // ── Reviews ─────────────────────────────────────────────────────────────
    public bool SubmitReview(int orderId, int userId, int productId, string content, int rating)
    {
        // Guard: order must exist, belong to user, and be Completed
        var order = _orderRepo.GetOrderById(orderId);
        if (order == null || order.UserId != userId || order.Status != "Completed") return false;
        if (_orderRepo.HasFeedbackForOrder(orderId, userId)) return false;

        return _feedbackRepo.AddFeedback(new Feedback
        {
            UserId    = userId,
            ProductId = productId,
            OrderId   = orderId,
            Content   = content,
            Rating    = rating,
            Status    = true,
            CreatedAt = DateTime.Now
        });
    }

    public bool HasFeedbackForOrder(int orderId, int userId)
        => _orderRepo.HasFeedbackForOrder(orderId, userId);
}
