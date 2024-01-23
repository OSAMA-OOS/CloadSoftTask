using Employee_Documents.Models.Entities;

namespace Employee_Documents.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void insert (Employee employee);
        void update (Employee employee);
        void delete (int id);
        void save ();
    }
}
