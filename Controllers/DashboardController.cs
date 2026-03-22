using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IAccountService   _accountService;

    public DashboardController(IDashboardService dashboardService, IAccountService accountService)
    { _dashboardService = dashboardService; _accountService = accountService; }

    // SDD: 3.6.1 View Admin Dashboard
    [RequireLogin("Admin")]
    public IActionResult Admin() => View(_dashboardService.GetAdminDashboard());

    // SDD: 3.5.1 View Sales Dashboard — Sale & Admin only
    [RequireLogin("Sale", "Admin")]
    public IActionResult Sales()
    {
        var role    = HttpContext.GetUserRole();
        var userId  = HttpContext.GetUserId()!.Value;
        int? saleId = role == "Sale" ? userId : null;
        return View(_dashboardService.GetSalesDashboard(saleId));
    }

    // SDD: 3.4.1 View Marketing Dashboard — Marketing & Admin only
    [RequireLogin("Marketing", "Admin")]
    public IActionResult Marketing() => View(_dashboardService.GetMarketingDashboard());

    // SDD: 3.6.2 Manage Users List
    [RequireLogin("Admin")]
    public IActionResult Users(string? search, int? roleId, bool? status)
    {
        ViewBag.Roles = _accountService.GetAllRoles();
        return View(_accountService.GetAllUsers(search, roleId, status));
    }

    [RequireLogin("Admin")]
    public IActionResult ToggleUser(int id)
    { _accountService.ToggleUserStatus(id); return RedirectToAction("Users"); }

    [RequireLogin("Admin")]
    public IActionResult EditUser(int id)
    {
        ViewBag.Roles = _accountService.GetAllRoles();
        return View(_accountService.GetCurrentUser(id));
    }

    [RequireLogin("Admin")]
    [HttpPost]
    public IActionResult EditUser(User user)
    { _accountService.UpdateUser(user); TempData["Success"] = "User updated."; return RedirectToAction("Users"); }

    [RequireLogin("Admin")]
    public IActionResult ResetPassword(int id)
    { _accountService.ResetPassword(id, "123"); TempData["Success"] = "Password reset to 123."; return RedirectToAction("Users"); }
}
