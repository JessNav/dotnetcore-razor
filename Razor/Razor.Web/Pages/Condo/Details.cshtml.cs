using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Razor.Web.Pages.Condo
{
    public class DetailsModel : PageModel
    {
        private readonly RazorAptDbContext _context;

        public DetailsModel(RazorAptDbContext context)
        {
            _context = context;
        }

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
    }
}
