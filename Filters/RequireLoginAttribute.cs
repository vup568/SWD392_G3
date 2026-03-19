using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SWD392_MVC.Services;

namespace SWD392_MVC.Filters;

public class RequireLoginAttribute : ActionFilterAttribute
{
    private readonly string[] _roles;

    public RequireLoginAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // 1. Read JWT from HttpOnly cookie
        var token = context.HttpContext.Request.Cookies["jwt"];

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        // 2. Validate the token
        var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
        var principal  = jwtService?.ValidateToken(token);

        if (principal == null)
        {
            // Token invalid or expired — clear cookie and redirect to login
            context.HttpContext.Response.Cookies.Delete("jwt");
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        // 3. Check role if required
        var userRole = principal.FindFirst("UserRole")?.Value;

        if (_roles.Length > 0 && !_roles.Contains(userRole))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        // 4. Attach claims to HttpContext so controllers can read them
        context.HttpContext.User = principal;

        base.OnActionExecuting(context);
    }
}
