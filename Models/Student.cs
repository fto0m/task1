using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "صيغة الايميل غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        // Many-to-Many مع Course عن طريق جدول الوصل StudentCourse
        public List<StudentCourse> StudentCourses { get; set; } = new();
    }
}
