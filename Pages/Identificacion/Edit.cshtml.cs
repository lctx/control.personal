using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using control.personal.Models;

namespace control.personal.Pages.Identificacion
{
    public class EditModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;

        public EditModel(control.personal.Data.ApplicationDbContext context)
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
           ViewData["idUsuario"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Identificacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentificacionExists(Identificacion.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IdentificacionExists(int id)
        {
            return _context.Identificacion.Any(e => e.id == id);
        }
    }
}
