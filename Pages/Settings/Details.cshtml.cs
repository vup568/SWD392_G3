using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShopWeb.Models;

namespace OnlineShopWeb.Pages_Settings
{
    public class DetailsModel : PageModel
    {
        private readonly OnlineShopWeb.Models.OnlineShopContext _context;

        public DetailsModel(OnlineShopWeb.Models.OnlineShopContext context)
        {
            _context = context;
        }

        public Setting Setting { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings.FirstOrDefaultAsync(m => m.SettingId == id);
            if (setting == null)
            {
                return NotFound();
            }
            else
            {
                Setting = setting;
            }
            return Page();
        }
    }
}
