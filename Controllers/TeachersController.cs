using CourseManagementApi.Data;
using CourseManagementApi.DTOs;
using CourseManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/teachers
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherReadDto>>> GetTeachers()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Courses)
                .Select(t => ToReadDto(t))
                .ToListAsync();

            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherReadDto>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound(new { message = $"لا يوجد مدرّس بالـ Id = {id}" });

            return Ok(ToReadDto(teacher));
        }

        [HttpPost]
        public async Task<ActionResult<TeacherReadDto>> CreateTeacher(TeacherCreateDto dto)
        {
            var teacher = new Teacher { FullName = dto.FullName, Email = dto.Email };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.Id }, ToReadDto(teacher));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, TeacherCreateDto dto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound(new { message = $"لا يوجد مدرّس بالـ Id = {id}" });

            teacher.FullName = dto.FullName;
            teacher.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound(new { message = $"لا يوجد مدرّس بالـ Id = {id}" });

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static TeacherReadDto ToReadDto(Teacher t) => new()
        {
            Id = t.Id,
            FullName = t.FullName,
            Email = t.Email,
            Courses = t.Courses?.Select(c => c.Title).ToList() ?? new()
        };
    }
}
