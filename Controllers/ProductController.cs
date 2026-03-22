using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService) { _productService = productService; }

    // Public
    public IActionResult Index(string? search, int? categoryId)
    {
        ViewBag.Categories = _productService.GetCategories();
        ViewBag.Search     = search;
        ViewBag.CategoryId = categoryId;
        return View(_productService.GetActiveProducts(search, categoryId));
    }

    public IActionResult Detail(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        return View(product);
    }

    // Product management moved to Sale (+ Admin)
    [RequireLogin("Sale", "Admin")]
    public IActionResult Manage(string? search, int? categoryId, bool? status)
    {
        ViewBag.Categories = _productService.GetCategories();
        return View(_productService.GetAllProducts(search, categoryId, status));
    }

    [RequireLogin("Sale", "Admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = _productService.GetCategories();
        return View();
    }

    [RequireLogin("Sale", "Admin")]
    [HttpPost]
    public IActionResult Create(Product product)
    {
        _productService.CreateProduct(product);
        TempData["Success"] = "Product created successfully.";
        return RedirectToAction("Manage");
    }

    [RequireLogin("Sale", "Admin")]
    public IActionResult Edit(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        ViewBag.Categories = _productService.GetCategories();
        return View(product);
    }

    [RequireLogin("Sale", "Admin")]
    [HttpPost]
    public IActionResult Edit(Product product)
    {
        _productService.UpdateProduct(product);
        TempData["Success"] = "Product updated successfully.";
        return RedirectToAction("Manage");
    }

    [RequireLogin("Sale", "Admin")]
    public IActionResult ToggleStatus(int id)
    {
        _productService.ToggleProductStatus(id);
        return RedirectToAction("Manage");
    }
}
