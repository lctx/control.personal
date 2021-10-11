using System;
using System.Collections.Generic;
using control.personal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace control.personal.Models
{
    public class CrearModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        public List<Usuario> Usuarios { get; set; }
        [BindProperty]
        public Byte[] QR { get; set; }
        public CrearModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            QR = null;
            Usuarios = new List<Usuario>();
            GetUsuarios();
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public void GetUsuarios()
        {
            var usuarios = _userManager.Users;
            foreach (var item in usuarios)
            {
                Usuarios.Add(item);
            }
        }
    }

}