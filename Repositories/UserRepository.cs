using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(OnlineShopContext context) : base(context) { }

    public User? GetUserByEmail(string email)
        => _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == email);

    public User? GetUserWithRole(int userId)
        => _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);

    public IList<User> GetUsers(string? search = null, int? roleId = null, bool? status = null)
    {
        var q = _context.Users.Include(u => u.Role).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(u => u.FullName.Contains(search) || u.Email.Contains(search));
        if (roleId.HasValue) q = q.Where(u => u.RoleId == roleId);
        if (status.HasValue) q = q.Where(u => u.Status == status);
        return q.ToList();
    }

    public IList<User> GetCustomers()
    {
        var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Customer")?.RoleId;
        return _context.Users.Include(u => u.Role).Where(u => u.RoleId == roleId).ToList();
    }

    public User? GetCustomerWithOrders(int userId)
        => _context.Users.Include(u => u.Role).Include(u => u.OrderUsers)
            .FirstOrDefault(u => u.UserId == userId);

    public bool ValidateLogin(string email, string password, out User? user)
    {
        user = _context.Users.Include(u => u.Role)
            .FirstOrDefault(u => u.Email == email && u.PasswordHash == password && u.Status == true);
        return user != null;
    }

    public bool UpdateProfile(User user)
    {
        var existing = _context.Users.Find(user.UserId);
        if (existing == null) return false;
        existing.FullName = user.FullName;
        existing.Phone    = user.Phone;
        existing.Address  = user.Address;
        _context.SaveChanges();
        return true;
    }

    public bool UpdatePassword(int userId, string oldPwd, string newPwd)
    {
        var user = _context.Users.Find(userId);
        if (user == null || user.PasswordHash != oldPwd) return false;
        user.PasswordHash = newPwd;
        _context.SaveChanges();
        return true;
    }

    public bool ResetPassword(int userId, string newPassword)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return false;
        user.PasswordHash = newPassword;
        _context.SaveChanges();
        return true;
    }

    public bool UpdateUserStatus(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return false;
        user.Status = !(user.Status ?? true);
        _context.SaveChanges();
        return true;
    }

    public bool CreateUser(User user)
    {
        if (CheckEmailExists(user.Email)) return false;
        user.CreatedAt = DateTime.Now;
        user.Status    = true;
        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateUser(User user)
    {
        var existing = _context.Users.Find(user.UserId);
        if (existing == null) return false;
        existing.FullName = user.FullName;
        existing.Email    = user.Email;
        existing.Phone    = user.Phone;
        existing.Address  = user.Address;
        existing.RoleId   = user.RoleId;
        existing.Status   = user.Status;
        _context.SaveChanges();
        return true;
    }

    public bool CheckEmailExists(string email)
        => _context.Users.Any(u => u.Email == email);
}
