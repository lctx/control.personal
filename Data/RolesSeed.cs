using System.Linq;
using System.Threading.Tasks;
using control.personal.Models;
using control.personal.Utils;
using Microsoft.AspNetCore.Identity;

namespace control.personal.Data
{
    public static class RolesSeed
    {
        public static async Task SeedRolesAsync(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Count() != 3)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Empleado.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Sensor.ToString()));
            }
        }
    }
}