using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Genealogy.Models;

namespace Genealogy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClanEventController : ControllerBase
    {
        private readonly GenealogyContext _context;

        public ClanEventController(GenealogyContext context)
        {
            _context = context;
        }

        // GET: api/ClanEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClanEventTable>>> GetClanEventTables()
        {
          if (_context.ClanEventTables == null)
          {
              return NotFound();
          }
            return await _context.ClanEventTables.ToListAsync();
        }

        // GET: api/ClanEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClanEventTable>> GetClanEventTable(string id)
        {
          if (_context.ClanEventTables == null)
          {
              return NotFound();
          }
            var clanEventTable = await _context.ClanEventTables.FindAsync(id);

            if (clanEventTable == null)
            {
                return NotFound();
            }

            return clanEventTable;
        }

        // PUT: api/ClanEvent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClanEventTable(string id, ClanEventTable clanEventTable)
        {
            if (id != clanEventTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(clanEventTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClanEventTableExists(id))
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

        // POST: api/ClanEvent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClanEventTable>> PostClanEventTable(ClanEventTable clanEventTable)
        {
          if (_context.ClanEventTables == null)
          {
              return Problem("Entity set 'GenealogyContext.ClanEventTables'  is null.");
          }
            _context.ClanEventTables.Add(clanEventTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClanEventTableExists(clanEventTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClanEventTable", new { id = clanEventTable.Id }, clanEventTable);
        }

        // DELETE: api/ClanEvent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClanEventTable(string id)
        {
            if (_context.ClanEventTables == null)
            {
                return NotFound();
            }
            var clanEventTable = await _context.ClanEventTables.FindAsync(id);
            if (clanEventTable == null)
            {
                return NotFound();
            }

            _context.ClanEventTables.Remove(clanEventTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClanEventTableExists(string id)
        {
            return (_context.ClanEventTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
