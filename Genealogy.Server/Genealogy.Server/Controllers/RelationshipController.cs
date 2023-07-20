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
    public class RelationshipController : ControllerBase
    {
        private readonly GenealogyContext _context;

        public RelationshipController(GenealogyContext context)
        {
            _context = context;
        }

        // GET: api/Relationship
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelationshipTable>>> GetRelationshipTables()
        {
          if (_context.RelationshipTables == null)
          {
              return NotFound();
          }
            return await _context.RelationshipTables.ToListAsync();
        }

        // GET: api/Relationship/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RelationshipTable>> GetRelationshipTable(string id)
        {
          if (_context.RelationshipTables == null)
          {
              return NotFound();
          }
            var relationshipTable = await _context.RelationshipTables.FindAsync(id);

            if (relationshipTable == null)
            {
                return NotFound();
            }

            return relationshipTable;
        }

        // PUT: api/Relationship/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelationshipTable(string id, RelationshipTable relationshipTable)
        {
            if (id != relationshipTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(relationshipTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipTableExists(id))
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

        // POST: api/Relationship
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RelationshipTable>> PostRelationshipTable(RelationshipTable relationshipTable)
        {
          if (_context.RelationshipTables == null)
          {
              return Problem("Entity set 'GenealogyContext.RelationshipTables'  is null.");
          }
            _context.RelationshipTables.Add(relationshipTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RelationshipTableExists(relationshipTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRelationshipTable", new { id = relationshipTable.Id }, relationshipTable);
        }

        // DELETE: api/Relationship/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelationshipTable(string id)
        {
            if (_context.RelationshipTables == null)
            {
                return NotFound();
            }
            var relationshipTable = await _context.RelationshipTables.FindAsync(id);
            if (relationshipTable == null)
            {
                return NotFound();
            }

            _context.RelationshipTables.Remove(relationshipTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RelationshipTableExists(string id)
        {
            return (_context.RelationshipTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
