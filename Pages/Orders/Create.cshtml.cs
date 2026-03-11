using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Orders
{
    public class CreateModel : PageModel
    {
        private readonly OnlineShopWeb.Models.OnlineShopContext _context;

        public CreateModel(OnlineShopWeb.Models.OnlineShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SaleId"] = new SelectList(_context.Users, "UserId", "UserId");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
