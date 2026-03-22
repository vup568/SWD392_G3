using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByEmail(string email);
    User? GetUserWithRole(int userId);
    IList<User> GetUsers(string? search = null, int? roleId = null, bool? status = null);
    IList<User> GetCustomers();
    User? GetCustomerWithOrders(int userId);
    bool ValidateLogin(string email, string password, out User? user);
    bool UpdateProfile(User user);
    bool UpdatePassword(int userId, string oldPwd, string newPwd);
    bool ResetPassword(int userId, string newPassword);
    bool UpdateUserStatus(int userId);
    bool CreateUser(User user);
    bool UpdateUser(User user);
    bool CheckEmailExists(string email);
}
