using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public IndexModel(OnlineShopContext context) { _context = context; }

        public IList<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

        public async Task OnGetAsync()
        {
            Categories = await _context.ProductCategories
                .Where(c => c.Status == true).ToListAsync();
            FeaturedProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Status == true)
                .Take(8).ToListAsync();
        }
    }
}
