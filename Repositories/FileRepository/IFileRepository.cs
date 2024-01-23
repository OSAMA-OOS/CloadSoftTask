using Employee_Documents.Models.Entities;
using File = Employee_Documents.Models.Entities.File;

namespace Employee_Documents.Repositories.FileRepository
{
    public interface IFileRepository
    {
        IEnumerable<File> GetAll();
        File GetById(int id);
        void insert(File employee);
        void update(File employee);
        void delete(int id);

        void save();
    }
}
