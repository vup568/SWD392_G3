using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Users
{
    public class CreateModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public CreateModel(OnlineShopContext context) { _context = context; }

        public IActionResult OnGet()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return Page();
        }

        [BindProperty]
        public new User UserData { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            _context.Users.Add(UserData);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
