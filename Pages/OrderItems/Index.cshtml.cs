using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_OrderItems
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopWeb.Models.OnlineShopContext _context;

        public IndexModel(OnlineShopWeb.Models.OnlineShopContext context)
        {
            _context = context;
        }

        public IList<OrderItem> OrderItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            OrderItem = await _context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.Product).ToListAsync();
        }
    }
}
