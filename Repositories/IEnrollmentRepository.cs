using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        bool Exists(int studentSsn, int courseId);
    }
}
