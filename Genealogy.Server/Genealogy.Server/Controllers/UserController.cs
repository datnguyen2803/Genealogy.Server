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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GenealogyContext _context;

        public UserController(GenealogyContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTable>> GetUserTable(string id)
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            var userTable = await _context.UserTables.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(string id, UserTable userTable)
        {
            if (id != userTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUserTable(UserTable userTable)
        {
          if (_context.UserTables == null)
          {
              return Problem("Entity set 'GenealogyContext.UserTables'  is null.");
          }
            _context.UserTables.Add(userTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTableExists(userTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserTable", new { id = userTable.Id }, userTable);
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<ActionResult<UserTable>> Register(string userName, string password)
        {
            if (_context.UserTables == null)
            {
                return Problem("Entity set 'GenealogyContext.UserTables'  is null.");
            }
            UserTable userTable = new UserTable()
            {
                Id = Guid.NewGuid().ToString(),
                Name = userName,
                Password = password,
                Email = "testEmail",
                Role = false
            };
            _context.UserTables.Add(userTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserTableExists(userTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserTable", new { id = userTable.Id }, userTable);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(string id)
        {
            if (_context.UserTables == null)
            {
                return NotFound();
            }
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(string id)
        {
            return (_context.UserTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
