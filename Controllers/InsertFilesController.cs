using Employee_Documents.Models.Entities;
using Employee_Documents.Repositories.EmployeeRepository;
using Employee_Documents.Repositories.FileRepository;
using Employee_Documents.Repositories.ReportRepository;
using Employee_Documents.ViewModel.InsertFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employee_Documents.Controllers
{
    public class InsertFilesController : Controller
    {
        private readonly IFileRepository FileRepository;
        private readonly IEmployeeRepository EmployeeRepository;
        private readonly IReportRepository ReportRepository;
        private readonly IWebHostEnvironment environment;
        public InsertFilesController(IFileRepository FileRepository , IEmployeeRepository EmployeeRepository , IReportRepository reportRepository , IWebHostEnvironment environment)
        {
            this.environment = environment;
            this.FileRepository = FileRepository;
            this.EmployeeRepository = EmployeeRepository;
            this.ReportRepository = reportRepository;
        }
        public IActionResult Index()
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.errorMessage = errorMessage;
            string successMessage = TempData["SuccessMessage"] as string;
            ViewBag.successMessage = successMessage;
            ViewData["EmployeesID"] = new SelectList(EmployeeRepository.GetAll(), "Id", "Name");
            ViewData["FilesID"] = new SelectList(FileRepository.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Insert(EmployeeFileVIewModel employeeFileVIewModel)
        {
            // Check if the Employee already has the File
            bool alreadyExists = ReportRepository.Exists(employeeFileVIewModel.EmployeeId, employeeFileVIewModel.FileId);

            if (alreadyExists)
            {
                // EmployeeFile already exists, set error message in TempData
                TempData["ErrorMessage"] = "This Employee already has the selected File.";
                return RedirectToAction("Index");
            }
            else
            {
                // Check if a file is uploaded
                if (employeeFileVIewModel.UploadedFile != null && employeeFileVIewModel.UploadedFile.Length > 0)
                {
                    // Save the file to a specific location
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "uploads");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + employeeFileVIewModel.UploadedFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        employeeFileVIewModel.UploadedFile.CopyTo(stream);
                    }

                    // Update the URL property with the file path
                    employeeFileVIewModel.URL = "/uploads/" + uniqueFileName;
                }
                else
                {
                    
                    TempData["ErrorMessage"] = "Please choose a file.";

                    return RedirectToAction("Index");
                }

                //var newEmployeeFile = new EmployeeFile
                //{
                //    EmployeeId = employeeFileVIewModel.EmployeeId,
                //    FileId = employeeFileVIewModel.FileId,
                //    URL = employeeFileVIewModel.URL
                //};

                TempData["SuccessMessage"] = "File added to the Employee successfully!";
                ReportRepository.insert(employeeFileVIewModel);

                return RedirectToAction("Index");
            }
        }

    }
}
