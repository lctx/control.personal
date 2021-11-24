using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using control.personal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace control.personal.Pages.Usuarios
{
    [Authorize(Policy = "Politica-Administrador")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        public IConfiguration _configuration { get; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Nombre { get; set; }

        public DeleteModel(UserManager<Usuario> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public IActionResult OnGet(string email, string nombre)
        {
            if (email == null)
            {
                return NotFound();
            }
            if (email == _configuration["Administracion:General"])
            {
                return RedirectToPage("Index");
            }
            Email = email;
            Nombre = nombre;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string email, string nombre)
        {
            if (email == null)
            {
                return NotFound();
            }

            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario != null)
            {
                await _userManager.DeleteAsync(usuario);
            }

            return RedirectToPage("./Index");
        }
    }
}