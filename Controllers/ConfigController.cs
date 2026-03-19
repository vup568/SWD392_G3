using Microsoft.AspNetCore.Mvc;
using SWD392_MVC.Filters;
using SWD392_MVC.Models;
using SWD392_MVC.Services;

namespace SWD392_MVC.Controllers;

[RequireLogin("Admin")]
public class ConfigController : Controller
{
    private readonly IConfigurationService _configService;
    public ConfigController(IConfigurationService configService) { _configService = configService; }

    public IActionResult Categories(string? search, bool? status) => View(_configService.GetCategories(search, status));
    public IActionResult CreateCategory() => View();
    [HttpPost] public IActionResult CreateCategory(ProductCategory cat) { _configService.CreateCategory(cat); return RedirectToAction("Categories"); }
    public IActionResult EditCategory(int id) => View(_configService.GetCategories().FirstOrDefault(c => c.CategoryId == id));
    [HttpPost] public IActionResult EditCategory(ProductCategory cat) { _configService.UpdateCategory(cat); return RedirectToAction("Categories"); }
    public IActionResult ToggleCategory(int id) { _configService.ToggleCategoryStatus(id); return RedirectToAction("Categories"); }

    public IActionResult Roles() => View(_configService.GetRoles());
    public IActionResult CreateRole() => View();
    [HttpPost] public IActionResult CreateRole(Role role) { _configService.CreateRole(role); return RedirectToAction("Roles"); }
    public IActionResult EditRole(int id) => View(_configService.GetRoles().FirstOrDefault(r => r.RoleId == id));
    [HttpPost] public IActionResult EditRole(Role role) { _configService.UpdateRole(role); return RedirectToAction("Roles"); }

    public IActionResult Settings(string? search) => View(_configService.GetSettings(search));
    public IActionResult CreateSetting() => View();
    [HttpPost] public IActionResult CreateSetting(Setting setting) { _configService.CreateSetting(setting); return RedirectToAction("Settings"); }
    public IActionResult EditSetting(int id) => View(_configService.GetSettings().FirstOrDefault(s => s.SettingId == id));
    [HttpPost] public IActionResult EditSetting(Setting setting) { _configService.UpdateSetting(setting); return RedirectToAction("Settings"); }
    public IActionResult ToggleSetting(int id) { _configService.ToggleSettingStatus(id); return RedirectToAction("Settings"); }
}
