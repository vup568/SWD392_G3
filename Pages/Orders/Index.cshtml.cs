using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Orders
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopWeb.Models.OnlineShopContext _context;

        public IndexModel(OnlineShopWeb.Models.OnlineShopContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == null ||
                (role != "Admin" && role != "SaleManager"))
            {
                return RedirectToPage("/Account/Login");
            }

            Order = await _context.Orders
                .Include(o => o.Sale)
                .Include(o => o.User)
                .ToListAsync();

            return Page();
        }
    }
}
