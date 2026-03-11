using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public LoginModel(OnlineShopContext context) { _context = context; }

        [BindProperty] public string Email { get; set; } = "";
        [BindProperty] public string Password { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToPage("/Index");
            return Page();
        }

        public IActionResult OnPost()
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == Email && u.PasswordHash == Password && u.Status == true);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role.RoleName);

            return RedirectToPage("/Index");
        }
    }
}
