using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public Course? FindByName(string name)
        {
            return _dbSet.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
        }

        public IEnumerable<Course> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return _dbSet.ToList();
            
            return _dbSet.Where(c => c.Name.Contains(query) || c.Description.Contains(query)).ToList();
        }
    }
}
