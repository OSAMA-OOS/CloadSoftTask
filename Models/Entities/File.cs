using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Documents.Models.Entities
{
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter File Name")]
        [MaxLength(50, ErrorMessage = "File Name Must be less than 50 char ")]
        [MinLength(3, ErrorMessage = "File Name Must be more than 3 char")]
        public string Name { get; set; }

        public ICollection<EmployeeFile> EmployeeFiles { get; set; } = new List<EmployeeFile>();
    }
}
