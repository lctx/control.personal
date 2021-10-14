using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using control.personal.Models;

namespace control.personal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IdentificacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Identificacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Identificacion>>> GetIdentificacion()
        {
            return await _context.Identificacion.ToListAsync();
        }

        // GET: api/Identificacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Identificacion>> GetIdentificacion(int id)
        {
            var identificacion = await _context.Identificacion.FindAsync(id);

            if (identificacion == null)
            {
                return NotFound();
            }

            return identificacion;
        }

        // PUT: api/Identificacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdentificacion(int id, Identificacion identificacion)
        {
            if (id != identificacion.id)
            {
                return BadRequest();
            }

            _context.Entry(identificacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdentificacionExists(id))
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

        // POST: api/Identificacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Identificacion>> PostIdentificacion(Identificacion identificacion)
        {
            _context.Identificacion.Add(identificacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIdentificacion", new { id = identificacion.id }, identificacion);
        }

        // DELETE: api/Identificacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIdentificacion(int id)
        {
            var identificacion = await _context.Identificacion.FindAsync(id);
            if (identificacion == null)
            {
                return NotFound();
            }

            _context.Identificacion.Remove(identificacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IdentificacionExists(int id)
        {
            return _context.Identificacion.Any(e => e.id == id);
        }
    }
}
