using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public Department? GetWithDetails(int id)
        {
            return _dbSet
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .FirstOrDefault(d => d.DeptId == id);
        }

        public IEnumerable<Department> GetAllWithDetails()
        {
            return _dbSet
                .Include(d => d.Students)
                .Include(d => d.Instructors)
                .ToList();
        }
    }
}
