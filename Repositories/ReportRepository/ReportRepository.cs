using Employee_Documents.Models.Context;
using Employee_Documents.Models.Entities;
using Employee_Documents.Repositories.EmployeeRepository;
using Employee_Documents.Repositories.FileRepository;
using Employee_Documents.ViewModel.InsertFiles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Employee_Documents.Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly EContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IFileRepository fileRepository;
        public ReportRepository(EContext context, IWebHostEnvironment webHostEnvironment, IEmployeeRepository employeeRepository , IFileRepository fileRepository)

        {
            this.fileRepository = fileRepository;
            this.employeeRepository = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.context = context;
        }

        public IEnumerable<EmployeeFile> GetAll()
        {
            var query = from employee in context.Employees
                        from fileName in context.Files
                        join employeeFile in context.EmployeeFiles
                            on new { EmployeeId = employee.Id, FileNameId = fileName.Id } equals new { EmployeeId = employeeFile.EmployeeId, FileNameId = employeeFile.FileId } into employeeFileGroup
                        from employeeFile in employeeFileGroup.DefaultIfEmpty()
                        orderby employee.Id
                        select new EmployeeFile
                        {
                            EmployeeId = employee.Id,
                            URL = (employeeFile != null) ? employeeFile.URL : string.Empty,
                            FileId = (employeeFile != null) ? employeeFile.FileId : 0,
                            // Include other properties from the EmployeeFile entity as needed
                            Employee = employee,
                            File = fileName
                        };

            return query.ToList();
        }




        public void insert(EmployeeFileVIewModel employeeFile)
        {
            var employeeName = employeeRepository.GetById(employeeFile.EmployeeId).Name;
            var fileName = fileRepository.GetById(employeeFile.FileId).Name;
            string uniqueFileName = UploadedFile(employeeFile.UploadedFile, employeeName + fileName) ;
            var newobject = new EmployeeFile();
            newobject.EmployeeId = employeeFile.EmployeeId;
            newobject.FileId = employeeFile.FileId;
            newobject.URL = uniqueFileName;
            context.EmployeeFiles.Add(newobject);
            context.SaveChanges();
        }
        public bool Exists(int employeeId, int fileId)
        {
            return context.EmployeeFiles.Any(ef => ef.EmployeeId == employeeId && ef.FileId == fileId);
        }

        public string UploadedFile(IFormFile model, string filename)
        {
            string uniqueFileName = null;
            if (model != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "FilesImages");
                uniqueFileName = filename + System.IO.Path.GetExtension(model.FileName).ToString();

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //if (System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Delete(filePath);
                //}

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public void Delete(EmployeeFile employeeFile)
        {
            var Record = GetbyId(employeeFile.EmployeeId,employeeFile.FileId);
            context.EmployeeFiles.Remove(Record);
            context.SaveChanges();
        }

        public EmployeeFile GetbyId(int empId, int fileid)
        {
            return context.EmployeeFiles.Include(e => e.Employee).Include(f => f.File).FirstOrDefault(emp => emp.EmployeeId == empId && emp.FileId == fileid);
        }


    }
}
