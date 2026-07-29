using CourseManagementApi.Data;
using CourseManagementApi.DTOs;
using CourseManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/students
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.StudentCourses).ThenInclude(sc => sc.Course)
                .Select(s => ToReadDto(s))
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentReadDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses).ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound(new { message = $"لا يوجد طالب بالـ Id = {id}" });

            return Ok(ToReadDto(student));
        }

        // POST: api/students
        [HttpPost]
        public async Task<ActionResult<StudentReadDto>> CreateStudent(StudentCreateDto dto)
        {
            var student = new Student { FullName = dto.FullName, Email = dto.Email };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, ToReadDto(student));
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentCreateDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = $"لا يوجد طالب بالـ Id = {id}" });

            student.FullName = dto.FullName;
            student.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound(new { message = $"لا يوجد طالب بالـ Id = {id}" });

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{studentId}/courses/{courseId}")]
        public async Task<IActionResult> EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
                return NotFound(new { message = $"لا يوجد طالب بالـ Id = {studentId}" });

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound(new { message = $"لا يوجد كورس بالـ Id = {courseId}" });

            var alreadyEnrolled = await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (alreadyEnrolled)
                return BadRequest(new { message = "الطالب مسجّل بهاد الكورس أصلاً" });

            _context.StudentCourses.Add(new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = $"تم تسجيل {student.FullName} بكورس {course.Title} بنجاح" });
        }

        [HttpDelete("{studentId}/courses/{courseId}")]
        public async Task<IActionResult> UnenrollStudentFromCourse(int studentId, int courseId)
        {
            var enrollment = await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

            if (enrollment == null)
                return NotFound(new { message = "الطالب مش مسجّل بهاد الكورس" });

            _context.StudentCourses.Remove(enrollment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static StudentReadDto ToReadDto(Student s) => new()
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            Courses = s.StudentCourses?.Select(sc => sc.Course.Title).ToList() ?? new()
        };
    }
}
