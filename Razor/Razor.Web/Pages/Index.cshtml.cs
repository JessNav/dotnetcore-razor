using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Pages
{
    public class IndexModel : PageModel
    {
        public RedirectToPageResult OnGet()
        {
            return RedirectToPage("/condo/list");
        }
    }
}
