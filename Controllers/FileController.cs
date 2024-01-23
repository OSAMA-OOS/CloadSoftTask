using Employee_Documents.Models.Entities;
using Employee_Documents.Repositories.EmployeeRepository;
using Employee_Documents.Repositories.FileRepository;
using Microsoft.AspNetCore.Mvc;
using File = Employee_Documents.Models.Entities.File;

namespace Employee_Documents.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileRepository FileRepository;
        public FileController(IFileRepository FileRepository)
        {
            this.FileRepository = FileRepository;
        }
        public IActionResult Index()
        {
            var Files = FileRepository.GetAll();
            return View(Files);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(File file)
        {
            FileRepository.insert(file);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var file = FileRepository.GetById(id);
            return View(file);
        }
        [HttpPost]
        public IActionResult Edit(File file)
        {
            FileRepository.update(file);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            FileRepository.delete(id);
            return RedirectToAction("Index");
        }
    }
}
