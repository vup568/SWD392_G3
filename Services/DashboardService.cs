using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

// ── DTOs ────────────────────────────────────────────────────────────────────
public class AdminDashboardData
{
    public int     TotalUsers       { get; set; }
    public int     TotalProducts    { get; set; }
    public int     TotalOrders      { get; set; }
    public decimal TotalRevenue     { get; set; }
    public int     NewOrdersToday   { get; set; }
    public Dictionary<string, int>        OrdersByStatus { get; set; } = new();
    public List<(string date, decimal revenue)> RevenueByDay { get; set; } = new();
}

public class SalesDashboardData
{
    public int     TotalOrders     { get; set; }
    public int     PendingOrders   { get; set; }
    public int     CompletedOrders { get; set; }
    public decimal TotalRevenue    { get; set; }
    public List<(string name, int qty)> TopProducts { get; set; } = new();
}

public class MarketingDashboardData
{
    public int TotalCustomers  { get; set; }
    public int TotalPosts      { get; set; }
    public int TotalFeedbacks  { get; set; }
    public int ActiveSliders   { get; set; }
    public List<(string category, int count)> ProductsByCategory { get; set; } = new();
}

// ── Interface ────────────────────────────────────────────────────────────────
public interface IDashboardService
{
    AdminDashboardData     GetAdminDashboard();
    SalesDashboardData     GetSalesDashboard(int? saleId = null);
    MarketingDashboardData GetMarketingDashboard();
}

// ── Implementation ───────────────────────────────────────────────────────────
public class DashboardService : IDashboardService
{
    private readonly IOrderRepository   _orderRepo;
    private readonly IUserRepository    _userRepo;
    private readonly IProductRepository _productRepo;
    private readonly IPostRepository    _postRepo;
    private readonly IFeedbackRepository _feedbackRepo;
    private readonly ISliderRepository  _sliderRepo;
    // Need raw context only for OrderItems top-products query
    private readonly OnlineShopContext  _context;

    public DashboardService(
        IOrderRepository    orderRepo,
        IUserRepository     userRepo,
        IProductRepository  productRepo,
        IPostRepository     postRepo,
        IFeedbackRepository feedbackRepo,
        ISliderRepository   sliderRepo,
        OnlineShopContext   context)
    {
        _orderRepo    = orderRepo;
        _userRepo     = userRepo;
        _productRepo  = productRepo;
        _postRepo     = postRepo;
        _feedbackRepo = feedbackRepo;
        _sliderRepo   = sliderRepo;
        _context      = context;
    }

    public AdminDashboardData GetAdminDashboard()
    {
        var today  = DateTime.Today;
        var cutoff = today.AddDays(-7);
        var orders = _orderRepo.GetAll();

        return new AdminDashboardData
        {
            TotalUsers     = _userRepo.GetAll().Count,
            TotalProducts  = _productRepo.GetProducts(status: true).Count,
            TotalOrders    = orders.Count,
            TotalRevenue   = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalAmount ?? 0),
            NewOrdersToday = orders.Count(o => o.OrderDate >= today),
            OrdersByStatus = orders.GroupBy(o => o.Status ?? "Unknown")
                                   .ToDictionary(g => g.Key, g => g.Count()),
            RevenueByDay   = orders
                .Where(o => o.Status == "Completed" && o.OrderDate >= cutoff)
                .GroupBy(o => o.OrderDate!.Value.Date)
                .Select(g => (g.Key.ToString("dd/MM"), g.Sum(o => o.TotalAmount ?? 0)))
                .OrderBy(x => x.Item1)
                .ToList()
        };
    }

    public SalesDashboardData GetSalesDashboard(int? saleId = null)
    {
        var orders = _orderRepo.GetAllOrders(saleId: saleId);

        var topProducts = _context.OrderItems
            .Include(oi => oi.Product)
            .AsEnumerable()
            .GroupBy(oi => oi.Product?.ProductName ?? "Unknown")
            .Select(g => (g.Key, g.Sum(x => x.Quantity ?? 0)))
            .OrderByDescending(x => x.Item2)
            .Take(5)
            .ToList();

        return new SalesDashboardData
        {
            TotalOrders     = orders.Count,
            PendingOrders   = orders.Count(o => o.Status == "Submitted"),
            CompletedOrders = orders.Count(o => o.Status == "Completed"),
            TotalRevenue    = orders.Where(o => o.Status == "Completed").Sum(o => o.TotalAmount ?? 0),
            TopProducts     = topProducts
        };
    }

    public MarketingDashboardData GetMarketingDashboard()
    {
        var products = _productRepo.GetActiveProducts()
            .GroupBy(p => p.Category?.CategoryName ?? "Uncategorized")
            .Select(g => (g.Key, g.Count()))
            .ToList();

        return new MarketingDashboardData
        {
            TotalCustomers     = _userRepo.GetCustomers().Count,
            TotalPosts         = _postRepo.CountPosts(),
            TotalFeedbacks     = _feedbackRepo.CountFeedbacks(),
            ActiveSliders      = _sliderRepo.CountActiveSliders(),
            ProductsByCategory = products
        };
    }
}
