using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementApi.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الكورس مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 12, ErrorMessage = "عدد الساعات المعتمدة لازم يكون بين 1 و 12")]
        public int CreditHours { get; set; }

        public int? TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher? Teacher { get; set; }

        public List<StudentCourse> StudentCourses { get; set; } = new();
    }
}
