using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using control.personal.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using control.personal.Utils;

namespace control.personal.Pages.Registro
{
    public class IndexModel : PageModel
    {
        private readonly control.personal.Data.ApplicationDbContext _context;
        public IConfiguration _configuration { get; }
        public bool administradorIdentificaciones { get; set; }

        public IndexModel(control.personal.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public PaginatedList<Models.Registro> RegistroPaginado { get; set; }

        public async Task OnGetAsync(DateTime? fecha, int? pageNumber)
        {
            var _administrador = _configuration["Administracion:Identificaciones"];
            var _usrActual = User.FindFirst(ClaimTypes.Email);
            //para pruebas se utilizó 1, cambiar por un valor estandar ejm 5
            int pageSize = 3;
            if (_usrActual != null)
            {
                administradorIdentificaciones = String.Equals(_administrador.ToLower(), _usrActual.Value.ToLower());
            }
            if (pageNumber == null)
            {
                pageNumber = 1;
            }
            if (fecha == null)
            {
                RegistroPaginado = await PaginatedList<Models.Registro>
                        .CreateAsync(
                            _context.Registro.Include(r => r.Usuario).Include(x => x.ControlIngresos).OrderBy(x => x.FechaHora)
                            , pageNumber ?? 1
                            , pageSize);
            }
            else
            {
                ViewData["fecha"] = fecha.Value.Date.ToString("yyyy-MM-dd");
                RegistroPaginado = await PaginatedList<Models.Registro>
                    .CreateAsync(
                        _context.Registro
                            .Where(x => DateTime.Compare(x.FechaHora.Date, fecha.Value.Date) == 0)
                            .Include(r => r.Usuario).OrderBy(x => x.FechaHora)
                        , pageNumber ?? 1
                        , pageSize);
            }
        }
    }
}
