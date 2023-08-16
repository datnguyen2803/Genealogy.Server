using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ExcelDataReader;
using Genealogy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using IronXL;
using System.Collections.Generic;

namespace Genealogy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly GenealogyContext _context;

        private enum TABLE
        {
            USER = 0,
            SERVER_EVENT,
            MEMBER,
            RELATIONSHIP,
            CLAN_EVENT
        }


        private readonly string[] ALLOWED_EXTIONSIONS = { ".xlsx", ".xls" };

        // USER TABLE
        private static readonly string[] EXCEL_COL_NAME = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P" };
        private enum USERTABLE_COL_INDEX
        {
            ID = 0,
            NAME,
            PASSWORD,
            EMAIL,
            ROLE
        }

        // MEMBER TABLE
        private enum MEMBER_COL_INDEX
        {
            ID = 0,
            SURNAME,
            LASTNAME,
            GENDER,
            DOB,
            DOD,
            BIRTH_PLACE,
            CURRENT_PLACE,
            IS_CLAN_LEADER,
            GEN_NO,
            IMAGE,
            NOTE,
        }

        //RELATIONSHIP TABLE
        private enum RELATIONSHIP_COL_INDEX
        {
            ID = 0,
            MAIN_MEM_ID,
            SUB_MEM_ID,
            RELATE_CODE,
            DATE_START
        }





        public FileController(GenealogyContext context)
        {
            this._context = context;
        }


        [HttpPost]
        public void ImportUserData()
        {

            ClearOldData(TABLE.USER);
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            string filePath = currentDir + @"..\..\..\Excel\Users.xlsx";
            WorkBook workBook = WorkBook.Load(filePath);
            WorkSheet workSheet = workBook.WorkSheets.First();

            int rowCount = workSheet.RowCount;
            for (int i = 2; i <= rowCount; i++ )
            {
                if (workSheet[EXCEL_COL_NAME[(int)USERTABLE_COL_INDEX.NAME] + i.ToString()].IsEmpty)
                {
                    continue;
                }
                UserTable user = new UserTable();
                user.Id = Guid.NewGuid().ToString();
                user.Name = workSheet[EXCEL_COL_NAME[(int)USERTABLE_COL_INDEX.NAME] + i.ToString()].StringValue;
                user.Password = workSheet[EXCEL_COL_NAME[(int)USERTABLE_COL_INDEX.PASSWORD] + i.ToString()].StringValue;
                user.Email = workSheet[EXCEL_COL_NAME[(int)USERTABLE_COL_INDEX.EMAIL] + i.ToString()].StringValue;
                user.Role = workSheet[EXCEL_COL_NAME[(int)USERTABLE_COL_INDEX.ROLE] + i.ToString()].IntValue == 1 ? UserTable.ROLE_ADMIN : UserTable.ROLE_NORMAL;

                _context.UserTables.Add(user);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public void ImportMemberData()
        {

            ClearOldData(TABLE.MEMBER);
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            string filePath = currentDir + @"..\..\..\Excel\Members.xlsx";
            WorkBook workBook = WorkBook.Load(filePath);
            WorkSheet workSheet = workBook.WorkSheets.First();

            int rowCount = workSheet.RowCount;
            for (int i = 2; i <= rowCount; i++)
            {
                //if (workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.SURNAME] + i.ToString()].IsEmpty)
                //{
                //    continue;
                //}
                MemberTable member = new MemberTable();
                member.Id = Guid.NewGuid().ToString();
                member.Surname = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.SURNAME] + i.ToString()].StringValue;
                member.Lastname = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.LASTNAME] + i.ToString()].StringValue;
                member.Gender = (byte)workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.GENDER] + i.ToString()].Int32Value;

                string dobStr = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.DOB] + i.ToString()].StringValue;
                member.Dob = dobStr != null ? DateTime.Parse(dobStr) : DateTime.MinValue;
                string dodStr = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.DOD] + i.ToString()].StringValue;
                member.Dod = dodStr != null ? DateTime.Parse(dodStr) : DateTime.MinValue;
                member.BirthPlace = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.BIRTH_PLACE] + i.ToString()].StringValue;
                member.CurrentPlace = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.CURRENT_PLACE] + i.ToString()].StringValue;
                member.IsClanLeader = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.IS_CLAN_LEADER] + i.ToString()].Int32Value == 1;
                member.GenNo = (ushort)workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.GEN_NO] + i.ToString()].Int32Value;
                member.Image = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.IMAGE] + i.ToString()].StringValue;
                member.Note = workSheet[EXCEL_COL_NAME[(int)MEMBER_COL_INDEX.NOTE] + i.ToString()].StringValue;

                _context.MemberTables.Add(member);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public void ImportRelationshipData()
        {

            ClearOldData(TABLE.RELATIONSHIP);
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            string filePath = currentDir + @"..\..\..\Excel\Relationships.xlsx";
            WorkBook workBook = WorkBook.Load(filePath);
            WorkSheet workSheet = workBook.WorkSheets.First();

            int rowCount = workSheet.RowCount;
            for (int i = 2; i <= rowCount; i++)
            {
                if (workSheet[EXCEL_COL_NAME[(int)RELATIONSHIP_COL_INDEX.MAIN_MEM_ID] + i.ToString()].IsEmpty)
                {
                    continue;
                }
                RelationshipTable relationship = new RelationshipTable();
                relationship.Id = Guid.NewGuid().ToString();
                relationship.MainMemId = workSheet[EXCEL_COL_NAME[(int)RELATIONSHIP_COL_INDEX.MAIN_MEM_ID] + i.ToString()].StringValue;
                relationship.SubMemId = workSheet[EXCEL_COL_NAME[(int)RELATIONSHIP_COL_INDEX.SUB_MEM_ID] + i.ToString()].StringValue;
                relationship.RelateCode = (byte)workSheet[EXCEL_COL_NAME[(int)RELATIONSHIP_COL_INDEX.RELATE_CODE] + i.ToString()].Int32Value;
                string dateStartStr = workSheet[EXCEL_COL_NAME[(int)RELATIONSHIP_COL_INDEX.DATE_START] + i.ToString()].StringValue;
                relationship.DateStart = dateStartStr != null ? DateTime.Parse(dateStartStr) : DateTime.MinValue;

                _context.RelationshipTables.Add(relationship);
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }
        }

        private void ClearOldData(TABLE table)
        {
            if(_context == null)
            {
                return;
            }
            switch(table)
            {
                case TABLE.USER:
                    {
                        if (_context.UserTables == null)
                        {
                            return;
                        }
                        foreach(var entity in _context.UserTables)
                        {
                            _context.UserTables.Remove(entity);
                        }
                        _context.SaveChanges();

                        break;
                    }
                case TABLE.SERVER_EVENT:
                    {
                        if (_context.ServerEventTables == null)
                        {
                            return;
                        }
                        foreach (var entity in _context.ServerEventTables)
                        {
                            _context.ServerEventTables.Remove(entity);
                        }
                        _context.SaveChanges();

                        break;
                    }
                case TABLE.MEMBER:
                    {
                        if (_context.MemberTables == null)
                        {
                            return;
                        }
                        foreach (var entity in _context.MemberTables)
                        {
                            _context.MemberTables.Remove(entity);
                        }
                        _context.SaveChanges();

                        break;
                    }
                case TABLE.RELATIONSHIP:
                    {
                        if (_context.RelationshipTables == null)
                        {
                            return;
                        }
                        foreach (var entity in _context.RelationshipTables)
                        {
                            _context.RelationshipTables.Remove(entity);
                        }
                        _context.SaveChanges();

                        break;
                    }
                case TABLE.CLAN_EVENT:
                    {
                        if (_context.ClanEventTables == null)
                        {
                            return;
                        }
                        foreach (var entity in _context.ClanEventTables)
                        {
                            _context.ClanEventTables.Remove(entity);
                        }
                        _context.SaveChanges();

                        break;
                    }
                default:
                    {
                        // do nothing
                        break;
                    }
            }

        }

    }
}
