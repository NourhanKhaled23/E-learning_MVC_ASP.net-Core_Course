using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            var model = _mapper.Map<List<CourseViewModel>>(courses);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new CourseViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(model);
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Course added successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(model);
        }
    }
}
