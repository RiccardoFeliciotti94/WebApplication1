using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.SQL;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication1.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtenteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtenteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Utente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utente>>> GetUtente()
        {
            return await _context.Utente.ToListAsync();
        }

        // GET: api/Utente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utente>> GetUtente(string id)
        {
            var utente = await _context.Utente.FindAsync(id);

            if (utente == null)
            {
                return NotFound();
            }

            return utente;
        }

        // PUT: api/Utente/5?nome=sda
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtente(string id,Utente nome)
        {
            var utente = await _context.Utente.FindAsync(id);

            if (utente == null)
            {
                return NotFound();
            }

            utente.Nome = nome.Nome;
            _context.Entry(utente).State = EntityState.Modified;       
            await _context.SaveChangesAsync();
           
            return NoContent();
        }

        // POST: api/Utente
        [HttpPost]
        public async Task<ActionResult<Utente>> PostUtente(Utente utente)
        {
            _context.Utente.Add(utente);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UtenteExists(utente.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetUtente", new { id = utente.Email }, utente);
        }

        // DELETE: api/Utente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Utente>> DeleteUtente(string id)
        {
            var utente = await _context.Utente.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }
            _context.Utente.Remove(utente);
            await _context.SaveChangesAsync();

            return utente;
        }

        private bool UtenteExists(string id)
        {
            return _context.Utente.Any(e => e.Email == id);
        }
    }
}
