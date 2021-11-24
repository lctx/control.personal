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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace control.personal.Pages.Registro
{
    public class EditModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public EditModel(control.personal.Data.ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.Registro Registro { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Registro = await _context.Registro
                .Include(r => r.Usuario)
                .Include(r => r.ControlIngresos)
                .FirstOrDefaultAsync(m => m.id == id);

            if (Registro == null)
            {
                return NotFound();
            }
            ViewData["idUsuario"] = new SelectList(_context.Users, "Id", "Id");
            var _usuario = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email).Value);
            ViewData["nombreUsuario"] = _usuario.Nombre;
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
            var _registro = await _context.Registro.Include(x => x.ControlIngresos).Where(x => x.id == Registro.id).FirstOrDefaultAsync();
            _registro.ControlIngresos = Registro.ControlIngresos;

            _context.Attach(_registro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroExists(Registro.id))
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

        private bool RegistroExists(int id)
        {
            return _context.Registro.Any(e => e.id == id);
        }
    }
}
