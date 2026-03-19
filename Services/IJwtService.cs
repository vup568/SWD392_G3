using System.Security.Claims;
using SWD392_MVC.Models;

namespace SWD392_MVC.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}
