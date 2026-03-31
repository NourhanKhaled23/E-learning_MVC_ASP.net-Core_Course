using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;

namespace WebApplication1.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context)
        {
        }

        public bool Exists(int studentSsn, int courseId)
        {
            return _dbSet.Any(e => e.StudentSsn == studentSsn && e.CourseId == courseId);
        }
    }
}
