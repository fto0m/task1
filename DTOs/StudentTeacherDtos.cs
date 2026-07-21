using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.DTOs
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Courses { get; set; } = new();
    }

    public class StudentCreateDto
    {
        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class TeacherReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Courses { get; set; } = new();
    }

    public class TeacherCreateDto
    {
        [Required(ErrorMessage = "اسم المدرّس مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
