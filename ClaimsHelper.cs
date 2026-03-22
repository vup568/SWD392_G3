using System.Security.Claims;

namespace SWD392_MVC;

/// <summary>
/// Extension methods to read JWT claims from HttpContext easily in controllers.
/// Usage: var userId = HttpContext.GetUserId();
/// </summary>
public static class ClaimsHelper
{
    public static int? GetUserId(this HttpContext context)
    {
        var value = context.User.FindFirst("UserId")?.Value;
        return int.TryParse(value, out var id) ? id : null;
    }

    public static string? GetUserName(this HttpContext context)
        => context.User.FindFirst("UserName")?.Value;

    public static string? GetUserRole(this HttpContext context)
        => context.User.FindFirst("UserRole")?.Value;

    public static string? GetEmail(this HttpContext context)
        => context.User.FindFirst("Email")?.Value;

    public static bool IsLoggedIn(this HttpContext context)
        => context.User.Identity?.IsAuthenticated == true
           || context.Request.Cookies.ContainsKey("jwt");
}
