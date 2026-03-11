using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Shop
{
    public class OrderSuccessModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public OrderSuccessModel(OnlineShopContext context) { _context = context; }

        public Order? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            Order = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (Order == null) return RedirectToPage("/Index");
            return Page();
        }
    }
}
