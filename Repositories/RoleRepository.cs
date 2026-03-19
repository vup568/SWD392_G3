using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Role? GetRoleByName(string name);
    IList<Role> GetRoles();
}

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(OnlineShopContext context) : base(context) { }

    public Role? GetRoleByName(string name)
        => _context.Roles.FirstOrDefault(r => r.RoleName == name);

    public IList<Role> GetRoles() => _context.Roles.ToList();
}
