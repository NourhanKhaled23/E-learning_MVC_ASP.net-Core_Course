using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }

        public Student? GetWithDetails(int id)
        {
            return _dbSet
                .Include(s => s.Department)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefault(s => s.Ssn == id);
        }

        public IEnumerable<Student> GetAllWithDepartments()
        {
            return _dbSet.Include(s => s.Department).ToList();
        }
    }
}
