using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ExcelDataReader;
using Genealogy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using IronXL;

namespace Genealogy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        IConfiguration configuration;
        IWebHostEnvironment hostEnvironment;
        GenealogyContext context;
        IExcelDataReader reader;

        private readonly string[] ALLOWED_EXTIONSIONS = { ".xlsx", ".xls" };

        //USER TABLE
        private static readonly string[] USERTABLE_COL = { "A", "B", "C", "D", "E" };
        private enum USERTABLE_COL_INDEX
        {
            ID = 0,
            NAME,
            PASSWORD,
            EMAIL,
            ROLE
        }



        public FileController(IConfiguration configuration, IWebHostEnvironment hostEnvironment, GenealogyContext context)
        {
            this.configuration = configuration;
            this.hostEnvironment = hostEnvironment;
            this.context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var customerResponseDetails = await context.UserTables.ToListAsync();
        //    return View(customerResponseDetails);
        //}

        //[HttpPost]
        public static void ImportExcelFile(/* IFormFile file */)
        {
            try
            {
                // Check the file is received

                //if(file == null)
                //{
                //    throw new Exception("File is invalid");
                //}

                // Create the Directory if it is not exist
                //string dirPath = Path.Combine(hostEnvironment.WebRootPath, "ReceivedReports");
                //if(!Directory.Exists(dirPath))
                //{
                //    Directory.CreateDirectory(dirPath);
                //}

                string filePath = @"D:\Giapha\Genealogy.Server\Users.xlsx";
                WorkBook workBook = WorkBook.Load(filePath);
                WorkSheet workSheet = workBook.WorkSheets.First();

                int rowCount = workSheet.RowCount;
                for (int i = 2; i <= rowCount; i++ )
                {
                    if (workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.ID] + i.ToString()].IsEmpty)
                    {
                        continue;
                    }
                    UserTable user = new UserTable();
                    user.Id = workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.ID] + i.ToString()].StringValue;
                    user.Name = workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.NAME] + i.ToString()].StringValue;
                    user.Password = workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.PASSWORD] + i.ToString()].StringValue;
                    user.Email = workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.EMAIL] + i.ToString()].StringValue;
                    user.Role = workSheet[USERTABLE_COL[(int)USERTABLE_COL_INDEX.ROLE] + i.ToString()].IntValue == 1 ? UserTable.ROLE_ADMIN : UserTable.ROLE_NORMAL;

                }






                //// Make sure the file is excel file
                //string dataFileName = Path.GetFileName(file.FileName);
                //string dataFileExtension = Path.GetExtension(dataFileName);

                //if(!ALLOWED_EXTIONSIONS.Contains(dataFileExtension))
                //{
                //    throw new Exception("This type of file is not allowed");
                //}
                //// Make a copy of the posted file from the received file
                //string saveToPath = Path.Combine(dirPath, dataFileName);
                //using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                //{
                //    file.CopyTo(stream);
                //}

                //// Use this to handle Encodeing differences in .NET Core
                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //// read the excel file
                //using (var stream = new FileStream(saveToPath, FileMode.Open))
                //{
                //    if(dataFileExtension == ".xls")
                //    {
                //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                //    }
                //    else 
                //    {
                //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //    }

                //    DataSet ds = new DataSet();
                //    ds = reader.AsDataSet();
                //    reader.Close();

                //    if (ds != null && ds.Tables.Count > 0)
                //    {
                //        DataTable userTable = ds.Tables[0];
                //        for(int i = 1; i < userTable.Rows.Count; i++)
                //        {
                //            UserTable user = new UserTable();
                //            user.Id = userTable.Rows[i][(int)USERTABLE_ROW.ID].ToString();
                //            user.Name = userTable.Rows[i][(int)USERTABLE_ROW.NAME].ToString();
                //            user.Password = userTable.Rows[i][(int)USERTABLE_ROW.PASSWORD].ToString();
                //            user.Email = userTable.Rows[i][(int)USERTABLE_ROW.EMAIL].ToString();
                //            user.Role = userTable.Rows[i][(int)USERTABLE_ROW.ROLE].ToString() == "1" ? true : false;

                //            Console.WriteLine(user);

                //            //await context.UserTables.AddAsync(user);
                //            //await context.SaveChangesAsync();
                //        }

                //    }
                //}

                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //return RedirectToAction("Index");
            }
        }

    }
}
