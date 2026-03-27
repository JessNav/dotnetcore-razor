using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Razor.Web.Models;

namespace Razor.Web.Pages.Condo
{
    public class EditModel : PageModel
    {
        private readonly RazorAptDbContext _context;

        public EditModel(RazorAptDbContext context)
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

            var condo =  await _context.Condos.FirstOrDefaultAsync(m => m.Id == id);
            if (condo == null)
            {
                return NotFound();
            }
            Condo = condo;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Condo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CondoExists(Condo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Condo/Details?id=" + Condo.Id);
        }

        private bool CondoExists(int id)
        {
            return _context.Condos.Any(e => e.Id == id);
        }
    }
}
