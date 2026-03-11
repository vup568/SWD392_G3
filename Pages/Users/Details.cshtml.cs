using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Users
{
    public class DetailsModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public DetailsModel(OnlineShopContext context) { _context = context; }

        public User UserData { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null) return NotFound();
            UserData = user;
            return Page();
        }
    }
}
