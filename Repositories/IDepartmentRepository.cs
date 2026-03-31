using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department? GetWithDetails(int id);
        IEnumerable<Department> GetAllWithDetails();
    }
}
