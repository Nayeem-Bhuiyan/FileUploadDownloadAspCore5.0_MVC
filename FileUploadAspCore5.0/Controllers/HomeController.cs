using FileUploadAspCore5._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FileUploadAspCore5._0.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;

namespace FileUploadAspCore5._0.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public readonly EmployeeDbContext db;
        public HomeController(EmployeeDbContext _db)
        {
            this.db = _db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Index()
        {
            List<Employee> empList = new List<Employee>();
            var DataList = await db.Employee.ToListAsync();
            foreach (var em in DataList)
            {
                Employee emp = new Employee()
                {
                    Id = em.Id,
                    EmpName = em.EmpName,
                    Email = em.Email,
                    FileName=em.FileName,
                    FileType=em.FileType,
                    FileByte=em.FileByte,
                    FilePath=em.FilePath,
                    FileFullPath=em.FileFullPath

                };
                empList.Add(emp);
            };


            return View(empList);
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<string>> PostNewEmployee([FromForm] VmEmployee vm)
        {

            //FileName Collect from IFormFile property
            var fileName = Path.GetFileName(vm.IDataFile.FileName);  //here FileName is Built in word* using System.IO;
            //File Type from from fileName
            var fileExtension = Path.GetExtension(fileName);
            //Create new FileName with Guid
            var UniqueFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);



            if (fileExtension.ToLower() == ".pdf" || fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", UniqueFileName);

                //Saving image in Folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.IDataFile.CopyToAsync(stream);
                }

                //Other Form Data and ImagePath saving in database
                Employee emp = new Employee()
                {
                    EmpName = vm.EmpName,
                    Email = vm.Email,
                    FileName =fileName,
                    FileType = fileExtension,
                    FilePath= "../UploadedFiles/" + UniqueFileName,
                    FileFullPath=filePath
                };



                //Saving Image in sql Data base in varbinary formate
                using (var target = new MemoryStream())
                {
                    vm.IDataFile.CopyTo(target);
                    emp.FileByte = target.ToArray();
                }


                db.Employee.Add(emp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }



        [HttpGet]
        public async Task<ActionResult<VmEmployee>> Edit(int? id)
        {
            Employee emp = await db.Employee.Where(e => e.Id == id).FirstOrDefaultAsync();

            VmEmployee vm = new VmEmployee()
            {
                Id = emp.Id,
                EmpName = emp.EmpName,
                Email = emp.Email,
                FileName = emp.FileName,
                FileType = emp.FileType,
                FileByte = emp.FileByte,
                FilePath = emp.FilePath,
                FileFullPath = emp.FileFullPath

            };


            return View(vm);
        }


        [HttpPost]
        public async Task<ActionResult<string>> EditPost([FromForm] VmEmployee vm)
        {
            if (vm.Id <= 0)
            {
                return BadRequest();
            }
            Employee DbEmp = await db.Employee.Where(e => e.Id == vm.Id).FirstOrDefaultAsync();
            if (DbEmp == null)
            {
                return NotFound();
            }
            //FileName Collect from IFormFile property
            var fileName = Path.GetFileName(vm.IDataFile.FileName);  //here FileName is Built in word* using System.IO;
              //here FileName is Built in word* using System.IO;
            //File Type from from fileName
            var fileExtension = Path.GetExtension(fileName);
            //Create new FileName with Guid
            var UniqueFileName = string.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);



            if (fileExtension.ToLower() == ".pdf" || fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".jpeg")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", UniqueFileName);

                //Saving image in Folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.IDataFile.CopyToAsync(stream);
                }



                DbEmp.Id = vm.Id;
                DbEmp.EmpName = vm.EmpName;
                DbEmp.Email = vm.Email;
                DbEmp.FileName = fileName;
                DbEmp.FileType = fileExtension;
                DbEmp.FilePath = "../UploadedFiles/" + UniqueFileName;
                DbEmp.FileFullPath = filePath;

                //Saving Image in sql Data base in varbinary formate
                using (var target = new MemoryStream())
                {
                    vm.IDataFile.CopyTo(target);
                    DbEmp.FileByte = target.ToArray();
                }


                db.Entry(DbEmp).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult<VmEmployee>> Details(int? id)
        {

            Employee emp = await db.Employee.Where(e => e.Id == id).FirstOrDefaultAsync();

            VmEmployee vm = new VmEmployee()
            {
                Id = emp.Id,
                EmpName = emp.EmpName,
                Email = emp.Email,
                FileName = emp.FileName,
                FileType = emp.FileType,
                FileByte = emp.FileByte,
                FilePath = emp.FilePath,
                FileFullPath = emp.FileFullPath

            };


            return View(vm);
        }

        [HttpGet]
        public async Task<ActionResult<VmEmployee>> Delete(int? id)
        {

            Employee emp = await db.Employee.Where(e => e.Id == id).FirstOrDefaultAsync();

            VmEmployee vm = new VmEmployee()
            {
                Id = emp.Id,
                EmpName = emp.EmpName,
                Email = emp.Email,
                FileName = emp.FileName,
                FileType = emp.FileType,
                FileByte = emp.FileByte,
                FilePath = emp.FilePath,
                FileFullPath = emp.FileFullPath

            };


            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult<string>> DeleteConfirm(int? id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }
            Employee emp = await db.Employee.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (emp == null)
            {
                return NotFound();
            }
            if (emp != null)
            {
                db.Employee.Remove(emp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }



        [HttpGet]
        public  FileResult Download(int id)
        {
            var file = db.Employee.Where(x => x.Id == id).FirstOrDefault();
            var fileName = file.FileName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(file.FileFullPath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }



















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
