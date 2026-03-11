using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Shop
{
    public class ProductsModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public ProductsModel(OnlineShopContext context) { _context = context; }

        public IList<Product> Products { get; set; } = new List<Product>();
        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        [BindProperty(SupportsGet = true)] public int? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)] public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.ProductCategories.Where(c => c.Status == true).ToListAsync();
            var query = _context.Products.Include(p => p.Category).Where(p => p.Status == true);
            if (CategoryId.HasValue) query = query.Where(p => p.CategoryId == CategoryId);
            if (!string.IsNullOrWhiteSpace(Search)) query = query.Where(p => p.ProductName.Contains(Search));
            Products = await query.ToListAsync();
        }
    }
}
