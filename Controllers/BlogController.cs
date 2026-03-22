using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService) { _blogService = blogService; }

    public IActionResult Index(string? search) => View(_blogService.GetActivePosts(search));

    public IActionResult Detail(int id)
    {
        var post = _blogService.GetById(id);
        if (post == null || post.Status != true) return NotFound();
        return View(post);
    }

    // Marketing & Admin — blog management stays with Marketing
    [RequireLogin("Marketing", "Admin")]
    public IActionResult Manage(string? search, bool? status) => View(_blogService.GetAllPosts(search, status));

    [RequireLogin("Marketing", "Admin")]
    public IActionResult Create() => View();

    [RequireLogin("Marketing", "Admin")]
    [HttpPost]
    public IActionResult Create(Post post)
    {
        _blogService.Create(post, HttpContext.GetUserId()!.Value);
        TempData["Success"] = "Post created.";
        return RedirectToAction("Manage");
    }

    [RequireLogin("Marketing", "Admin")]
    public IActionResult Edit(int id) => View(_blogService.GetById(id));

    [RequireLogin("Marketing", "Admin")]
    [HttpPost]
    public IActionResult Edit(Post post)
    { _blogService.Update(post); TempData["Success"] = "Post updated."; return RedirectToAction("Manage"); }

    [RequireLogin("Marketing", "Admin")]
    public IActionResult ToggleStatus(int id) { _blogService.ToggleStatus(id); return RedirectToAction("Manage"); }

    [RequireLogin("Marketing", "Admin")]
    public IActionResult Delete(int id) { _blogService.Delete(id); return RedirectToAction("Manage"); }
}
