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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace control.personal.Pages.Usuarios
{
    [Authorize(Policy = "Politica-Administrador")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IConfiguration _configuration { get; }
        public Usuario Usuario { get; set; }

        public List<ManejoRoles> Roles { get; set; }
        public EditModel(ApplicationDbContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            Usuario = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (Usuario == null)
            {
                return NotFound();
            }
            Roles = new List<ManejoRoles>();
            foreach (var rol in _roleManager.Roles.ToList())
            {
                var rolActual = new ManejoRoles()
                {
                    RoleId = rol.Id,
                    RoleName = rol.Name
                };
                if (await _userManager.IsInRoleAsync(Usuario, rol.Name))
                {
                    rolActual.Elejido = true;
                }
                else
                {
                    rolActual.Elejido = false;
                }
                Roles.Add(rolActual);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(List<ManejoRoles> Roles, Usuario Usuario)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Roles == null)
            {
                return Page();
            }
            var _usr = await _userManager.FindByEmailAsync(Usuario.Email);
            var roles = await _userManager.GetRolesAsync(_usr);
            var result = await _userManager.RemoveFromRolesAsync(_usr, roles);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(_usr, Roles.Where(x => x.Elejido).Select(x => x.RoleName));
                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }
            }
            return NotFound();
        }
    }
    public class ManejoRoles
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Elejido { get; set; }
    }
}