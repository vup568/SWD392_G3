using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

[RequireLogin("Marketing", "Admin")]
public class MarketingController : Controller
{
    private readonly IMarketingService _marketingService;
    public MarketingController(IMarketingService marketingService) { _marketingService = marketingService; }

    public IActionResult Sliders() => View(_marketingService.GetAllSliders());
    public IActionResult CreateSlider() => View();
    [HttpPost] public IActionResult CreateSlider(Slider slider) { _marketingService.CreateSlider(slider); return RedirectToAction("Sliders"); }
    public IActionResult EditSlider(int id) => View(_marketingService.GetSliderById(id));
    [HttpPost] public IActionResult EditSlider(Slider slider) { _marketingService.UpdateSlider(slider); return RedirectToAction("Sliders"); }
    public IActionResult ToggleSlider(int id) { _marketingService.ToggleSliderStatus(id); return RedirectToAction("Sliders"); }

    public IActionResult Customers(string? search) => View(_marketingService.GetCustomers(search));
    public IActionResult CustomerDetail(int id) => View(_marketingService.GetCustomerById(id));

    public IActionResult Feedbacks(string? search, int? productId, bool? status)
        => View(_marketingService.GetAllFeedbacks(search, productId, status));
    public IActionResult ToggleFeedback(int id) { _marketingService.ToggleFeedbackStatus(id); return RedirectToAction("Feedbacks"); }
}
