using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using control.personal.Data;
using control.personal.Models;
using control.personal.Utils;
using Microsoft.AspNetCore.Authorization;

namespace control.personal.Pages.Identificacion
{
    /// <summary>
    /// Esta página será visible unicamente para
    /// </summary>
    [Authorize(Policy = "Politica-AdministracionIdentificaciones")]
    public class CreateModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;

        public CreateModel(control.personal.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string uid)
        {
            if (uid == null)
            {
                return NotFound();
            }
            //si ya existe la identificacion redirigir a los detalles de la misma
            var _identificacion=_context.Identificacion.Where(x => x.Uid==uid);
            if (_identificacion.Any())
            {
                return RedirectToPage("Edit",new {id = _identificacion.FirstOrDefault().id});
            }
            ViewData["idUsuario"] = new SelectList(_context.Users, "Id", "Nombre");
            ViewData["IdEstado"] = new SelectList(Enum.GetNames(typeof(EstadoIdentificacion)).ToList());
            ViewData["IdTipo"] = new SelectList(Enum.GetNames(typeof(TipoRFID)).ToList());
            Identificacion = new Models.Identificacion();
            Identificacion.Uid = uid;
            return Page();
        }

        [BindProperty]
        public control.personal.Models.Identificacion Identificacion { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Identificacion.Add(Identificacion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}