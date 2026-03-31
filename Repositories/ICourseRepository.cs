using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Course? FindByName(string name);
        IEnumerable<Course> Search(string query);
    }
}
