using Employee_Documents.Models.Entities;
using Employee_Documents.Repositories.EmployeeRepository;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Documents.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            var employees = employeeRepository.GetAll();
            return View(employees);  
        }

        [HttpGet]
        public IActionResult Create()
        {          
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            employeeRepository.insert(emp);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = employeeRepository.GetById(id);
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            employeeRepository.update(emp);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id) 
        {
            employeeRepository.delete(id);
            return RedirectToAction("Index");
        }

    }
}
