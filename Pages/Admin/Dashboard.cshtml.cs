using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly OnlineShopContext _context;

        public DashboardModel(OnlineShopContext context)
        {
            _context = context;
        }

        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == null || role != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            TotalUsers = _context.Users.Count();
            TotalProducts = _context.Products.Count();
            TotalOrders = _context.Orders.Count();
            TotalRevenue = _context.Orders.Sum(o => o.TotalAmount ?? 0);

            return Page();
        }
    }
}