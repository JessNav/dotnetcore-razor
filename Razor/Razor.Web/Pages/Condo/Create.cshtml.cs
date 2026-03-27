
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Web.Pages.Condo
{
    public class CreateModel : PageModel
    {
        private readonly RazorAptDbContext _context;

        public CreateModel(RazorAptDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Condo Condo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Condos.Add(Condo);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Condo/Details?id=" + Condo.Id);
        }
    }
}
