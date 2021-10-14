using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using control.personal.Models;

namespace control.personal.Pages.Identificacion
{
    public class IndexModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;

        public IndexModel(control.personal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<control.personal.Models.Identificacion> Identificacion { get;set; }

        public async Task OnGetAsync()
        {
            Identificacion = await _context.Identificacion
                .Include(i => i.Usuario).ToListAsync();
        }
    }
}
