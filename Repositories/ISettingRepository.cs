using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface ISettingRepository : IRepository<Setting>
{
    IList<Setting> GetSettings(string? search = null);
    bool CreateSetting(Setting setting);
    bool UpdateSetting(Setting setting);
    bool ToggleStatus(int id);
}
