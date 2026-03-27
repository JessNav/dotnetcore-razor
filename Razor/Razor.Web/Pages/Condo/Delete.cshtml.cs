using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor.Web.Models;

namespace Razor.Web.Pages.Condo
{
    public class DeleteModel : PageModel
    {
        private readonly RazorAptDbContext _context;

        public DeleteModel(RazorAptDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Condo Condo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condo = await _context.Condos.FirstOrDefaultAsync(m => m.Id == id);

            if (condo is not null)
            {
                Condo = condo;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condo = await _context.Condos.FindAsync(id);
            if (condo != null)
            {
                Condo = condo;
                _context.Condos.Remove(Condo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
