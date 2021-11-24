using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using control.personal.Data;
using control.personal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.personal.Pages.Usuarios
{
    [Authorize(Policy = "Politica-Administrador")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;
        public List<UsuarioRol> Usuarios;
        public IndexModel(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            Usuarios = new List<UsuarioRol>();
            _userManager.Users.ToList()
            .ForEach(
                usuario =>
                {
                    Usuarios.Add(new UsuarioRol() { Usuario = usuario });
                }
            );
        }
        public void OnGet()
        {
            foreach (var usuario in Usuarios)
            {
                usuario.Roles = GetUserRoles(usuario.Usuario).Result;
            }
        }
        private async Task<List<string>> GetUserRoles(Usuario user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
    public class UsuarioRol
    {
        public Usuario Usuario { get; set; }
        public List<string> Roles { get; set; }
    }

}