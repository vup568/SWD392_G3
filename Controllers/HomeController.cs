using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Repositories;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class HomeController : Controller
{
    private readonly IProductService    _productService;
    private readonly IBlogService       _blogService;
    private readonly ISliderRepository  _sliderRepo;
    private readonly IFeedbackRepository _feedbackRepo;

    public HomeController(
        IProductService     productService,
        IBlogService        blogService,
        ISliderRepository   sliderRepo,
        IFeedbackRepository feedbackRepo)
    {
        _productService = productService;
        _blogService    = blogService;
        _sliderRepo     = sliderRepo;
        _feedbackRepo   = feedbackRepo;
    }

    public IActionResult Index()
    {
        ViewBag.FeaturedProducts = _productService.GetActiveProducts().Take(8).ToList();
        ViewBag.Categories       = _productService.GetCategories();
        ViewBag.LatestPosts      = _blogService.GetActivePosts().Take(3).ToList();
        ViewBag.Sliders          = _sliderRepo.GetActiveSliders();
        // Top 6 visible reviews ordered by rating desc, then date
        ViewBag.TopReviews       = _feedbackRepo
            .GetFeedbacks(status: true)
            .OrderByDescending(f => f.Rating)
            .ThenByDescending(f => f.CreatedAt)
            .Take(6)
            .ToList();
        return View();
    }

    public IActionResult Error() => View();
}
