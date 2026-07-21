using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.Models
{
    // كل Teacher ممكن يدرّس أكتر من Course (One-to-Many)
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المدرّس مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "صيغة الايميل غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        // Navigation property: الكورسات اللي بيدرّسها هاد المدرّس
        public List<Course> Courses { get; set; } = new();
    }
}
