using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IAccountService
{
    User? Login(string email, string password);
    bool  Register(User user);
    User? GetCurrentUser(int userId);
    bool  UpdateUserProfile(User user);
    bool  ChangePassword(int userId, string oldPwd, string newPwd);
    bool  ResetPassword(int userId, string newPassword);
    IList<User> GetAllUsers(string? search = null, int? roleId = null, bool? status = null);
    bool  ToggleUserStatus(int userId);
    bool  CreateUser(User user);
    bool  UpdateUser(User user);
    IList<Role> GetAllRoles();
}

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;

    public AccountService(IUserRepository userRepo, IRoleRepository roleRepo)
    { _userRepo = userRepo; _roleRepo = roleRepo; }

    public User? Login(string email, string password)
    {
        _userRepo.ValidateLogin(email, password, out var user);
        return user;
    }

    public bool Register(User user)
    {
        if (_userRepo.CheckEmailExists(user.Email)) return false;
        user.CreatedAt = DateTime.Now;
        user.Status    = true;
        user.RoleId    = _roleRepo.GetRoleByName("Customer")?.RoleId ?? 1;
        _userRepo.Add(user);
        return true;
    }

    public User? GetCurrentUser(int userId) => _userRepo.GetUserWithRole(userId);

    public bool UpdateUserProfile(User user) => _userRepo.UpdateProfile(user);

    public bool ChangePassword(int userId, string oldPwd, string newPwd)
        => _userRepo.UpdatePassword(userId, oldPwd, newPwd);

    public bool ResetPassword(int userId, string newPassword)
        => _userRepo.ResetPassword(userId, newPassword);

    public IList<User> GetAllUsers(string? search = null, int? roleId = null, bool? status = null)
        => _userRepo.GetUsers(search, roleId, status);

    public bool ToggleUserStatus(int userId) => _userRepo.UpdateUserStatus(userId);

    public bool CreateUser(User user) => _userRepo.CreateUser(user);

    public bool UpdateUser(User user) => _userRepo.UpdateUser(user);

    public IList<Role> GetAllRoles() => _roleRepo.GetRoles();
}
