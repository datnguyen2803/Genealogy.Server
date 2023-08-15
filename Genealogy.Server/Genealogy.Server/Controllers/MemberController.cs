using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Genealogy.Models;
using Genealogy.Common;

namespace Genealogy.Controllers
{
    public class ClientMember
    {
        public string Surname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public byte Gender { get; set; }

        public DateTime? Dob { get; set; }

        public DateTime? Dod { get; set; }

        public string? BirthPlace { get; set; }

        public string? CurrentPlace { get; set; }

        public bool IsClanLeader { get; set; }

        public ushort GenNo { get; set; }

        public string? Image { get; set; }

        public string? Note { get; set; }

        public MemberTable Convert2MemberTable()
        {
            return new MemberTable()
            {
                Id = Guid.NewGuid().ToString(),
                Surname = Surname,
                Lastname = Lastname,
                Gender = Gender,
                Dob = Dob,
                Dod = Dod,
                BirthPlace = BirthPlace,
                CurrentPlace = CurrentPlace,
                IsClanLeader = IsClanLeader,
                GenNo = GenNo,
                Image = Image,
                Note = Note,
            };
        }

    }


    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly GenealogyContext _context;

        public MemberController(GenealogyContext context)
        {
            _context = context;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberTable>>> GetAll()
        {
            //ResponseModel response = new ResponseModel();
          if (_context.MemberTables == null)
          {
              return NotFound();
          }

            return await _context.MemberTables.ToListAsync();
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberTable>> GetMemberTable(string id)
        {
          if (_context.MemberTables == null)
          {
              return NotFound();
          }
            var memberTable = await _context.MemberTables.FindAsync(id);

            if (memberTable == null)
            {
                return NotFound();
            }

            return memberTable;
        }

        // PUT: api/Member/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberTable(string id, MemberTable memberTable)
        {
            if (id != memberTable.Id)
            {
                return BadRequest();
            }

            _context.Entry(memberTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberTableExists(id))
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

        //// POST: api/Member
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<MemberTable>> PostMemberTable(MemberTable memberTable)
        //{
        //  if (_context.MemberTables == null)
        //  {
        //      return Problem("Entity set 'GenealogyContext.MemberTables'  is null.");
        //  }
        //    _context.MemberTables.Add(memberTable);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (MemberTableExists(memberTable.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetMemberTable", new { id = memberTable.Id }, memberTable);
        //}

        // POST: api/Member
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("Add")]
        public async Task<ActionResult<MemberTable>> AddMember([FromBody] ClientMember clientMember)
        {
            if (_context.MemberTables == null)
            {
                return Problem("Entity set 'GenealogyContext.MemberTables'  is null.");
            }

            var memberTable = clientMember.Convert2MemberTable();

            _context.MemberTables.Add(memberTable);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MemberTableExists(memberTable.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMemberTable", new { id = memberTable.Id }, memberTable);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberTable(string id)
        {
            if (_context.MemberTables == null)
            {
                return NotFound();
            }
            var memberTable = await _context.MemberTables.FindAsync(id);
            if (memberTable == null)
            {
                return NotFound();
            }

            _context.MemberTables.Remove(memberTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberTableExists(string id)
        {
            return (_context.MemberTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
