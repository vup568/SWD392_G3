using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Shop
{
    public class CartModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public CartModel(OnlineShopContext context) { _context = context; }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total => CartItems.Sum(i => i.Subtotal);

        public class CartItem
        {
            public Product Product { get; set; } = null!;
            public int Quantity { get; set; }
            public decimal Subtotal => Product.Price * Quantity;
        }

        public async Task OnGetAsync()
        {
            await LoadCart();
        }

        public IActionResult OnGetAdd(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            cart[productId] = cart.ContainsKey(productId) ? cart[productId] + 1 : 1;
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            cart.Remove(productId);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            if (quantity <= 0) cart.Remove(productId);
            else cart[productId] = quantity;
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage();
        }

        private async Task LoadCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>("Cart") ?? new Dictionary<int, int>();
            if (!cart.Any()) return;
            var products = await _context.Products.Where(p => cart.Keys.Contains(p.ProductId)).ToListAsync();
            CartItems = products.Select(p => new CartItem { Product = p, Quantity = cart[p.ProductId] }).ToList();
        }
    }
}
