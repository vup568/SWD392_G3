using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Shop
{
    public class ProductDetailModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public ProductDetailModel(OnlineShopContext context) { _context = context; }

        public Product? Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (Product == null) return RedirectToPage("/Shop/Products");
            return Page();
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            cart[productId] = cart.ContainsKey(productId) ? cart[productId] + 1 : 1;
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage("/Shop/Cart");
        }
    }
}
