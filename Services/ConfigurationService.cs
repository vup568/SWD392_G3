using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IConfigurationService
{
    // Categories
    IList<ProductCategory> GetCategories(string? search = null, bool? status = null);
    bool CreateCategory(ProductCategory cat);
    bool UpdateCategory(ProductCategory cat);
    bool ToggleCategoryStatus(int id);

    // Roles
    IList<Role> GetRoles();
    bool CreateRole(Role role);
    bool UpdateRole(Role role);

    // Settings
    IList<Setting> GetSettings(string? search = null);
    bool CreateSetting(Setting setting);
    bool UpdateSetting(Setting setting);
    bool ToggleSettingStatus(int id);
}

public class ConfigurationService : IConfigurationService
{
    private readonly IProductCategoryRepository _categoryRepo;
    private readonly IRoleRepository            _roleRepo;
    private readonly ISettingRepository         _settingRepo;

    public ConfigurationService(
        IProductCategoryRepository categoryRepo,
        IRoleRepository            roleRepo,
        ISettingRepository         settingRepo)
    {
        _categoryRepo = categoryRepo;
        _roleRepo     = roleRepo;
        _settingRepo  = settingRepo;
    }

    public IList<ProductCategory> GetCategories(string? search = null, bool? status = null)
        => _categoryRepo.GetAllCategories(search, status);
    public bool CreateCategory(ProductCategory cat) => _categoryRepo.CreateCategory(cat);
    public bool UpdateCategory(ProductCategory cat) => _categoryRepo.UpdateCategory(cat);
    public bool ToggleCategoryStatus(int id)        => _categoryRepo.ToggleStatus(id);

    public IList<Role> GetRoles()    => _roleRepo.GetRoles();
    public bool CreateRole(Role r)   { _roleRepo.Add(r); return true; }
    public bool UpdateRole(Role r)
    {
        var e = _roleRepo.GetById(r.RoleId);
        if (e == null) return false;
        e.RoleName = r.RoleName;
        _roleRepo.Update(e);
        return true;
    }

    public IList<Setting> GetSettings(string? search = null) => _settingRepo.GetSettings(search);
    public bool CreateSetting(Setting s)  => _settingRepo.CreateSetting(s);
    public bool UpdateSetting(Setting s)  => _settingRepo.UpdateSetting(s);
    public bool ToggleSettingStatus(int id) => _settingRepo.ToggleStatus(id);
}
