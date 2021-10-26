using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using control.personal.Models;
using control.personal.Utils;
using control.personal.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace control.personal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TelegramService _telegramService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegistroController> _logger;

        public RegistroController(ApplicationDbContext context, IConfiguration configuration,
            TelegramService telegramService, ILogger<RegistroController> logger)
        {
            _context = context;
            _configuration = configuration;
            _telegramService = telegramService;
            _logger = logger;
        }

        // GET: api/Registro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Registro>>> GetRegistro()
        {
            return await _context.Registro.ToListAsync();
        }

        // GET: api/Registro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Registro>> GetRegistro(int id)
        {
            var registro = await _context.Registro.FindAsync(id);

            if (registro == null)
            {
                return NotFound();
            }

            return registro;
        }

        // PUT: api/Registro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistro(int id, Registro registro)
        {
            if (id != registro.id)
            {
                return BadRequest();
            }

            _context.Entry(registro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<string>> PostRegistro(string uid)
        {
            if (uid.Length != 11)
            {
                return new JsonResult(new Respuesta() { Error = "UID invalida" });
            }
            Usuario UsuarioIdentificado =
                await _context.Users
                .Include(x => x.Identificaciones)
                .Where(x => x.Identificaciones.Any(y => String.Equals(y.Uid, uid)))
                .FirstOrDefaultAsync();

            //verificar si con el null no da errores ya que puede generarse el objeto pero no contener nada por lo que no seria == a null
            if (UsuarioIdentificado == null)
            {
                //en este if se debe hacer el proceso para generar la url que se envia a administracion para asociar el uid a un usuario
                //puede usarse esto https://docs.microsoft.com/es-es/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0
                // con un cliente con tipo registrado en startup
                var _url = Url.Page(
                    "/Identificacion/Create",
                    pageHandler: null,
                    values: new { uid = uid },
                    protocol: Request.Scheme
                    );
                var telegram = await _telegramService.SendMessage(_telegramService.UrlBodyFormateado(_url, "Nueva identificación detectada", "Registro"));
                _logger.LogInformation(telegram);
                return new JsonResult(new Respuesta() { Error = "Identificación no encontrada" });
            }
            if (UsuarioIdentificado.Identificaciones
                .Where(x => String.Equals(x.Uid, uid))
                .Select(x => x.Estado)
                .FirstOrDefault() == EstadoIdentificacion.Activa)
            {
                //Crear un objeto de tipo registro y guardarlo en bd 
                Registro registro = new Registro() { FechaHora = DateTime.Now, idUsuario = UsuarioIdentificado.Id };
                await _context.Registro.AddAsync(registro);
                var result = await _context.SaveChangesAsync();
                return new JsonResult(new Respuesta() { FechaHora = DateTime.Now.ToString(), Nombre = UsuarioIdentificado.Nombre });
            }
            else
            {
                return new JsonResult(new Respuesta() { Error = "Identificación inactiva" });
            }
        }
        /*
                // POST: api/Registro
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Registro>> PostRegistro(Registro registro)
                {
                    _context.Registro.Add(registro);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetRegistro", new { id = registro.id }, registro);
                }
        */
        // DELETE: api/Registro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistro(int id)
        {
            var registro = await _context.Registro.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }

            _context.Registro.Remove(registro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroExists(int id)
        {
            return _context.Registro.Any(e => e.id == id);
        }
        class Respuesta
        {
            public string Error { get; set; }
            public string Nombre { get; set; }
            public string FechaHora { get; set; }
        }
    }
}
