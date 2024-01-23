using Employee_Documents.Models.Entities;
using System.Collections.Generic;
using File = Employee_Documents.Models.Entities.File;
using System.ComponentModel.DataAnnotations;

namespace Employee_Documents.ViewModel.InsertFiles
{
    public class EmployeeFileVIewModel
    {
        public int EmployeeId { get; set; }

        public int FileId { get; set; }
        [Required(ErrorMessage = "Please choose File")]
        public string URL { get; set; }

        public IFormFile UploadedFile { get; set; }

    }
}
