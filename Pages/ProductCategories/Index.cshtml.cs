using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_ProductCategories
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopWeb.Models.OnlineShopContext _context;

        public IndexModel(OnlineShopWeb.Models.OnlineShopContext context)
        {
            _context = context;
        }

        public IList<ProductCategory> ProductCategory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductCategory = await _context.ProductCategories.ToListAsync();
        }
    }
}
