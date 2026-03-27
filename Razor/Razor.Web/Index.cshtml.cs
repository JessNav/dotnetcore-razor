using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor;
using Razor.Web.Models;

namespace Razor.Web
{
    public class IndexModel : PageModel
    {
        private readonly Razor.RazorAptDbContext _context;

        public IndexModel(Razor.RazorAptDbContext context)
        {
            _context = context;
        }

        public IList<Condo> Condo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Condo = await _context.Condos.ToListAsync();
        }
    }
}
