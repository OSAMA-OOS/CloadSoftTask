using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Documents.Models.Entities
{
    public class EmployeeFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please choose File")]
        public string URL { get; set; }
        public virtual Employee Employee { get; set; }

        public int FileId { get; set; }
        public virtual File File { get; set; }
    }
}
