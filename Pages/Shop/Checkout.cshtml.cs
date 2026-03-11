using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages.Shop
{
    public class CheckoutModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public CheckoutModel(OnlineShopContext context) { _context = context; }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total => CartItems.Sum(i => i.Subtotal);

        [BindProperty] public string FullName { get; set; } = "";
        [BindProperty] public string Phone { get; set; } = "";
        [BindProperty] public string Address { get; set; } = "";
        [BindProperty] public string? Note { get; set; }

        public class CartItem
        {
            public Product Product { get; set; } = null!;
            public int Quantity { get; set; }
            public decimal Subtotal => Product.Price * Quantity;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            var user = await _context.Users.FindAsync(userId);
            if (user != null) { FullName = user.FullName; Phone = user.Phone ?? ""; Address = user.Address ?? ""; }

            await LoadCart();
            if (!CartItems.Any()) return RedirectToPage("/Shop/Cart");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            await LoadCart();
            if (!CartItems.Any()) return RedirectToPage("/Shop/Cart");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = Total,
                Status = "Submitted"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in CartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
                var product = await _context.Products.FindAsync(item.Product.ProductId);
                if (product != null) product.Quantity -= item.Quantity;
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            return RedirectToPage("/Shop/OrderSuccess", new { orderId = order.OrderId });
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
