using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IJwtService     _jwtService;

    public AccountController(IAccountService accountService, IJwtService jwtService)
    { _accountService = accountService; _jwtService = jwtService; }

    // SDD: 3.2.1 User Login
    public IActionResult Login()
    {
        if (Request.Cookies.ContainsKey("jwt")) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _accountService.Login(email, password);
        if (user == null) { ViewBag.Error = "Invalid email or password."; return View(); }
        var token = _jwtService.GenerateToken(user);
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true, Secure = false, SameSite = SameSiteMode.Strict,
            Expires  = DateTimeOffset.UtcNow.AddMinutes(60)
        });
        return RedirectToAction("Index", "Home");
    }

    // SDD: 3.2.2 User Register
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (!_accountService.Register(user))
        { ViewBag.Error = "Email already exists."; return View(); }
        TempData["Success"] = "Registered successfully! Please login.";
        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View();

    // SDD: 3.2.5 View User Profile
    [RequireLogin]
    public IActionResult Profile()
    {
        var userId = HttpContext.GetUserId();
        if (userId == null) return RedirectToAction("Login");
        return View(_accountService.GetCurrentUser(userId.Value));
    }

    // SDD: 3.2.6 Update User Profile
    [RequireLogin]
    [HttpPost]
    public IActionResult Profile(User user)
    {
        _accountService.UpdateUserProfile(user);
        TempData["Success"] = "Profile updated.";
        return RedirectToAction("Profile");
    }

    // SDD: 3.2.4 Change Password
    [RequireLogin]
    public IActionResult ChangePassword() => View();

    [RequireLogin]
    [HttpPost]
    public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword) { ViewBag.Error = "Passwords do not match."; return View(); }
        var userId = HttpContext.GetUserId();
        if (userId == null) return RedirectToAction("Login");
        if (!_accountService.ChangePassword(userId.Value, oldPassword, newPassword))
        { ViewBag.Error = "Current password is incorrect."; return View(); }
        TempData["Success"] = "Password changed. Please login again.";
        Response.Cookies.Delete("jwt");
        return RedirectToAction("Login");
    }
}
