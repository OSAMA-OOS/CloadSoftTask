using Employee_Documents.Models.Entities;
using Employee_Documents.ViewModel.InsertFiles;

namespace Employee_Documents.Repositories.ReportRepository
{
    public interface IReportRepository
    {
        IEnumerable<EmployeeFile> GetAll();
        void insert(EmployeeFileVIewModel employeeFile);
        bool Exists(int employeeId, int fileId);
        EmployeeFile GetbyId(int empId, int fileid);
        void Delete(EmployeeFile file);
    }
}
