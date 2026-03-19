using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService) { _orderService = orderService; }

    // SDD: 3.3.1 View My Orders
    [RequireLogin("Customer")]
    public IActionResult MyOrders()
        => View(_orderService.GetOrdersForUser(HttpContext.GetUserId()!.Value));

    // SDD: 3.3.2 View Order Information
    [RequireLogin("Customer")]
    public IActionResult MyOrderDetail(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null || order.UserId != HttpContext.GetUserId()) return NotFound();
        ViewBag.AlreadyReviewed = _orderService.HasFeedbackForOrder(id, HttpContext.GetUserId()!.Value);
        return View(order);
    }

    // SDD: 3.3.4 Cancel Submitted Order
    [RequireLogin("Customer")]
    public IActionResult Cancel(int id)
    {
        var userId = HttpContext.GetUserId()!.Value;
        if (!_orderService.CancelOrder(id, userId))
            TempData["Error"] = "Only submitted orders can be cancelled.";
        else
            TempData["Success"] = "Order cancelled successfully.";
        return RedirectToAction("MyOrders");
    }

    // NEW: Customer submits a review on a Completed order
    [RequireLogin("Customer")]
    [HttpPost]
    public IActionResult SubmitReview(int orderId, int productId, string content, int rating)
    {
        var userId = HttpContext.GetUserId()!.Value;
        if (!_orderService.SubmitReview(orderId, userId, productId, content, rating))
            TempData["Error"] = "Could not submit review. Order must be completed and not yet reviewed.";
        else
            TempData["Success"] = "Thank you for your review!";
        return RedirectToAction("MyOrderDetail", new { id = orderId });
    }

    // SDD: 3.5.2 Manage Orders List  — Sale & Admin only (SaleManager removed)
    [RequireLogin("Sale", "Admin")]
    public IActionResult Manage(string? status)
    {
        var role    = HttpContext.GetUserRole();
        var userId  = HttpContext.GetUserId()!.Value;
        int? saleId = role == "Sale" ? userId : null;
        ViewBag.StatusFilter = status;
        return View(_orderService.GetAllOrders(status, saleId));
    }

    // SDD: 3.5.3 View Order Details
    [RequireLogin("Sale", "Admin")]
    public IActionResult Detail(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return View(order);
    }

    // SDD: 3.5.4 Update Assigned Order
    [RequireLogin("Sale", "Admin")]
    [HttpPost]
    public IActionResult UpdateStatus(int orderId, string status)
    {
        var saleId = HttpContext.GetUserId()!.Value;
        _orderService.UpdateOrderStatus(orderId, status, saleId);
        TempData["Success"] = "Order status updated.";
        return RedirectToAction("Detail", new { id = orderId });
    }
}
