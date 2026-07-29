using CourseManagementApi.Data;
using CourseManagementApi.DTOs;
using CourseManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/courses
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseReadDto>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.StudentCourses)
                .Select(c => ToReadDto(c))
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseReadDto>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.StudentCourses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound(new { message = $"لا يوجد كورس بالـ Id = {id}" });

            return Ok(ToReadDto(course));
        }

        // POST: api/courses
        [HttpPost]
        public async Task<ActionResult<CourseReadDto>> CreateCourse(CourseCreateDto dto)
        {
            if (dto.TeacherId.HasValue)
            {
                var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == dto.TeacherId);
                if (!teacherExists)
                    return BadRequest(new { message = $"لا يوجد مدرّس بالـ Id = {dto.TeacherId}" });
            }

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                CreditHours = dto.CreditHours,
                TeacherId = dto.TeacherId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            await _context.Entry(course).Reference(c => c.Teacher).LoadAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, ToReadDto(course));
        }

        // PUT: api/courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseUpdateDto dto)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { message = $"لا يوجد كورس بالـ Id = {id}" });

            if (dto.TeacherId.HasValue)
            {
                var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == dto.TeacherId);
                if (!teacherExists)
                    return BadRequest(new { message = $"لا يوجد مدرّس بالـ Id = {dto.TeacherId}" });
            }

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.CreditHours = dto.CreditHours;
            course.TeacherId = dto.TeacherId;

            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound(new { message = $"لا يوجد كورس بالـ Id = {id}" });

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static CourseReadDto ToReadDto(Course c) => new()
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            CreditHours = c.CreditHours,
            TeacherId = c.TeacherId,
            TeacherName = c.Teacher?.FullName,
            EnrolledStudentsCount = c.StudentCourses?.Count ?? 0
        };
    }
}
