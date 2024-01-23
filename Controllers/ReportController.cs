using Employee_Documents.Repositories.EmployeeRepository;
using Employee_Documents.Repositories.FileRepository;
using Employee_Documents.Repositories.ReportRepository;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO.Compression;

namespace Employee_Documents.Controllers
{
    public class ReportController : Controller
    {
        private readonly IFileRepository FileRepository;
        private readonly IEmployeeRepository EmployeeRepository;
        private readonly IReportRepository ReportRepository;
        private readonly IWebHostEnvironment environment;
        public ReportController(IFileRepository FileRepository, IEmployeeRepository EmployeeRepository, IReportRepository ReportRepository, IWebHostEnvironment environment)
        {
            this.FileRepository = FileRepository;
            this.EmployeeRepository = EmployeeRepository;
            this.ReportRepository = ReportRepository;
            this.environment = environment;

        }
        public IActionResult Index()
        {
            string SuccessMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = SuccessMessage;
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.errorMessage = errorMessage;
            var EmpWithFiles = ReportRepository.GetAll();
            return View(EmpWithFiles);
        }
        [HttpGet]
        public IActionResult Delete(int empId, int fileid)
        {
            var record = ReportRepository.GetbyId(empId, fileid);
            if (record != null)
            {
                // Delete the associated file
                if (!string.IsNullOrEmpty(record.URL))
                {
                    string uploadsFolder = "FilesImages"; 
                    string filePath = Path.Combine(environment.WebRootPath, uploadsFolder, record.URL.TrimStart('/'));



                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        
                       
                        return BadRequest();
                    }
                }

                // Delete the record from the database
                ReportRepository.Delete(record);

                TempData["SuccessMessage"] = "File deleted successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "You Cannot Delete file that has no URL.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult GenerateReport()
        {

            var Records = ReportRepository.GetAll();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Employee Documents Report");


                var headerStyle = worksheet.Cells[1, 1, 1, 3].Style;
                headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                headerStyle.Font.Color.SetColor(System.Drawing.Color.White);

                worksheet.Cells[1, 1].Value = "Employee Name";
                worksheet.Cells[1, 2].Value = "File Name";
                worksheet.Cells[1, 3].Value = "URL";


                worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;


                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 20;
                worksheet.Column(3).Width = 20;




                int row = 2;
                foreach (var Record in Records)
                {
                    worksheet.Cells[row, 1].Value = Record.Employee.Name;
                    worksheet.Cells[row, 2].Value = Record.File.Name;
                    worksheet.Cells[row, 3].Value = Record.URL;



                    row++;
                }
                using (var cells = worksheet.Cells[2, 1, row - 1, 2])
                {
                    cells.Style.Numberformat.Format = "dd/MM/yyyy";
                }
                var excelBytes = package.GetAsByteArray();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employee Documents Report.xlsx");
            }
        }
    }
}
