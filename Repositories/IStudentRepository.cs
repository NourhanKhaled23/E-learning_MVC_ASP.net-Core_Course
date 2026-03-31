using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Student? GetWithDetails(int id);
        IEnumerable<Student> GetAllWithDepartments();
    }
}
