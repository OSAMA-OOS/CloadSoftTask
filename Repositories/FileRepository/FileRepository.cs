using Employee_Documents.Models.Context;
using Employee_Documents.Models.Entities;
using File = Employee_Documents.Models.Entities.File;

namespace Employee_Documents.Repositories.FileRepository
{
    public class FileRepository : IFileRepository
    {
        private readonly EContext context;
        public FileRepository(EContext context)
        {
            this.context = context;
        }
        public void delete(int id)
        {
            var file = GetById(id);

            if (file != null)
            {
                // Remove related files from the EmployeeFiles table
                context.EmployeeFiles.RemoveRange(context.EmployeeFiles.Where(empFile => empFile.FileId == id));

                // Remove the employee from the Employees table
                context.Files.Remove(file);

                save();
            }
        }

        public IEnumerable<Models.Entities.File> GetAll()
        {
            return context.Files.ToList();
        }

        public Models.Entities.File GetById(int id)
        {
            return context.Files.FirstOrDefault(file => file.Id == id);
        }

        public void insert(Models.Entities.File file)
        {
            var filee = new File();
            filee.Name = file.Name;
            context.Files.Add(filee);
            save();
        }

        public void save()
        {
            context.SaveChanges();
        }

        public void update(Models.Entities.File file)
        {
            var existingFile = GetById(file.Id);
            if (existingFile != null)
            {
                existingFile.Name = file.Name;
                save();
            }
        }
    }
}
