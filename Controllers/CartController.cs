using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class CartController : Controller
{
    private readonly IOrderService _orderService;
    public CartController(IOrderService orderService) { _orderService = orderService; }

    // SDD: 3.1.6 View Cart Details
    public IActionResult Index()
    {
        var role = HttpContext.GetUserRole();
        if (role != null && role != "Customer")
        {
            TempData["Error"] = "Staff accounts cannot use the shopping cart.";
            return RedirectToAction("Index", "Product");
        }
        return View(_orderService.GetCartItems(HttpContext.Session));
    }

    public IActionResult Add(int id, int quantity = 1)
    {
        var role = HttpContext.GetUserRole();
        if (role != null && role != "Customer")
        {
            TempData["Error"] = "Staff accounts cannot add items to cart.";
            return RedirectToAction("Detail", "Product", new { id });
        }
        _orderService.AddToCart(HttpContext.Session, id, quantity);
        TempData["Success"] = "Added to cart!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        _orderService.UpdateCart(HttpContext.Session, productId, quantity);
        return RedirectToAction("Index");
    }

    public IActionResult Remove(int id)
    {
        _orderService.RemoveFromCart(HttpContext.Session, id);
        return RedirectToAction("Index");
    }

    // SDD: 3.1.7 Enter Cart Contact Information
    [RequireLogin("Customer")]
    public IActionResult Checkout()
    {
        var items = _orderService.GetCartItems(HttpContext.Session);
        if (!items.Any()) return RedirectToAction("Index");
        return View(items);
    }

    // SDD: 3.1.8 Complete Order
    [RequireLogin("Customer")]
    [HttpPost]
    public IActionResult PlaceOrder()
    {
        var userId = HttpContext.GetUserId();
        if (userId == null) return RedirectToAction("Login", "Account");
        var order = _orderService.CreateOrder(userId.Value, HttpContext.Session);
        return RedirectToAction("OrderSuccess", new { id = order.OrderId });
    }

    public IActionResult OrderSuccess(int id)
        => View(_orderService.GetOrderById(id));
}
