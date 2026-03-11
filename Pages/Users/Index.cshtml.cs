using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Users
{
    public class IndexModel : PageModel
    {
        private readonly OnlineShopContext _context;
        public IndexModel(OnlineShopContext context) { _context = context; }

        public IList<User> UserList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            UserList = await _context.Users.Include(u => u.Role).ToListAsync();
        }
    }
}
