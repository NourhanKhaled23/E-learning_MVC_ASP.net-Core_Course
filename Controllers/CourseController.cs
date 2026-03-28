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

        public async Task<IActionResult> GetAll(string search = "")
        {
            var coursesQuery = _context.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                coursesQuery = coursesQuery.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            }

            var courses = await coursesQuery.ToListAsync();
            var model = _mapper.Map<List<CourseViewModel>>(courses);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CourseTable", model);
            }

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
                var isDuplicate = await _context.Courses.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower());
                if (isDuplicate)
                {
                    ModelState.AddModelError("Name", "A course with this name already exists.");
                    return View(model);
                }

                var course = _mapper.Map<Course>(model);
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Course added successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            var model = _mapper.Map<CourseViewModel>(course);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourseViewModel model)
        {
            if (id != model.CrsId) return NotFound();

            if (ModelState.IsValid)
            {
                var isDuplicate = await _context.Courses.AnyAsync(c => c.Name.ToLower() == model.Name.ToLower() && c.CrsId != id);
                if (isDuplicate)
                {
                    ModelState.AddModelError("Name", "Another course with this name already exists.");
                    return View(model);
                }

                var course = _mapper.Map<Course>(model);
                _context.Update(course);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Course updated successfully!";
                return RedirectToAction(nameof(GetAll));
            }
            return View(model);
        }
    }
}
