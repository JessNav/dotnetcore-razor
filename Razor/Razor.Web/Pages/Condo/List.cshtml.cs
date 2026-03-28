using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Razor.Web.Pages.Condo
{
    public class ListModel : PageModel
    {
        private readonly RazorAptDbContext _context;

        public List<Models.Condo> Condos = new();

        public ListModel(RazorAptDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Condos = await _context.Condos.ToListAsync();

            return Page();
        }
    }
}
