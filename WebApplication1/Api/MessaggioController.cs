using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.SQL;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication1.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MessaggioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessaggioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Messaggio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Messaggio>>> GetMessaggio()
        {
            return await _context.Messaggio.ToListAsync();
        }

        // GET: api/Messaggio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Messaggio>> GetMessaggio(string id)
        {
            var messaggio = await _context.Messaggio.FindAsync(id);

            if (messaggio == null)
            {
                return NotFound();
            }

            return messaggio;
        }


        // PUT: api/Messaggio/5?test=sdsad
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessaggio(string id, string testo)
        {
            var messaggio = await _context.Messaggio.FindAsync(id);

            if (messaggio == null)
            {
                return NotFound();
            }
            messaggio.Testo = testo;
            _context.Entry(messaggio).State = EntityState.Modified;

           
            await _context.SaveChangesAsync();
            

            return NoContent();
        }

        // POST: api/Messaggio
        [HttpPost]
        public async Task<ActionResult<Messaggio>> PostMessaggio(Messaggio messaggio)
        {
            _context.Messaggio.Add(messaggio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessaggio", new { id = messaggio.IDMessaggio }, messaggio);
        }

        // DELETE: api/Messaggio/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Messaggio>> DeleteMessaggio(string id)
        {
            var messaggio = await _context.Messaggio.FindAsync(id);
            if (messaggio == null)
            {
                return NotFound();
            }

            _context.Messaggio.Remove(messaggio);
            await _context.SaveChangesAsync();

            return messaggio;
        }

        private bool MessaggioExists(string id)
        {
            return _context.Messaggio.Any(e => e.IDMessaggio == id);
        }
    }
}
