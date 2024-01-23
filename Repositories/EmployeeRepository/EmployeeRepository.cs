using Employee_Documents.Models.Context;
using Employee_Documents.Models.Entities;

namespace Employee_Documents.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EContext context;
        public EmployeeRepository(EContext context )
        {
            this.context = context;
        }
        public void delete(int id)
        {
            var employee = GetById(id);

            if (employee != null)
            {
                // Remove related files from the EmployeeFiles table
                context.EmployeeFiles.RemoveRange(context.EmployeeFiles.Where(empFile => empFile.EmployeeId == id));

                // Remove the employee from the Employees table
                context.Employees.Remove(employee);

                save();
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            return context.Employees.ToList();
        }

        public Employee GetById(int id)
        {
            return context.Employees.FirstOrDefault(emp => emp.Id == id);
        }

        public void insert(Employee employee)
        {
            var emp = new Employee();
            emp.Name = employee.Name;
            context.Employees.Add( emp );
            save();

        }

        public void save()
        {
            context.SaveChanges();
        }

        public void update(Employee employee)
        {
            var existingEmployee = GetById(employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                save();
            }
        }
    }
}
