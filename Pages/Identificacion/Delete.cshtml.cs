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
    public class DeleteModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;

        public DeleteModel(control.personal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public control.personal.Models.Identificacion Identificacion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Identificacion = await _context.Identificacion
                .Include(i => i.Usuario).FirstOrDefaultAsync(m => m.id == id);

            if (Identificacion == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Identificacion = await _context.Identificacion.FindAsync(id);

            if (Identificacion != null)
            {
                _context.Identificacion.Remove(Identificacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
