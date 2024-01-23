using Employee_Documents.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using File = Employee_Documents.Models.Entities.File;

namespace Employee_Documents.Models.Context
{
    public class EContext : DbContext
        
    {
        public EContext() : base()
        {
            
        }
        public EContext(DbContextOptions<EContext> options):base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<EmployeeFile> EmployeeFiles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=osama;Initial Catalog=EmployeeFiles;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

    }
}
