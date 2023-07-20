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
    public class ServerEventController : ControllerBase
    {
        private readonly GenealogyContext _context;

        public ServerEventController(GenealogyContext context)
        {
            _context = context;
        }

        // GET: api/ServerEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServerEventTable>>> GetServerEventTables()
        {
          if (_context.ServerEventTables == null)
          {
              return NotFound();
          }
            return await _context.ServerEventTables.ToListAsync();
        }

        // GET: api/ServerEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerEventTable>> GetServerEventTable(string id)
        {
          if (_context.ServerEventTables == null)
          {
              return NotFound();
          }
            var serverEventTable = await _context.ServerEventTables.FindAsync(id);

            if (serverEventTable == null)
            {
                return NotFound();
            }

            return serverEventTable;
        }

        // PUT: api/ServerEvent/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServerEventTable(string id, ServerEventTable serverEventTable)
        {
            if (id != serverEventTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(serverEventTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServerEventTableExists(id))
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

        // POST: api/ServerEvent
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServerEventTable>> PostServerEventTable(ServerEventTable serverEventTable)
        {
          if (_context.ServerEventTables == null)
          {
              return Problem("Entity set 'GenealogyContext.ServerEventTables'  is null.");
          }
            _context.ServerEventTables.Add(serverEventTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServerEventTableExists(serverEventTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServerEventTable", new { id = serverEventTable.Id }, serverEventTable);
        }

        // DELETE: api/ServerEvent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServerEventTable(string id)
        {
            if (_context.ServerEventTables == null)
            {
                return NotFound();
            }
            var serverEventTable = await _context.ServerEventTables.FindAsync(id);
            if (serverEventTable == null)
            {
                return NotFound();
            }

            _context.ServerEventTables.Remove(serverEventTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServerEventTableExists(string id)
        {
            return (_context.ServerEventTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
