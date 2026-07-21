using System.ComponentModel.DataAnnotations;

namespace CourseManagementApi.DTOs
{
    // اللي بيرجع للمستخدم (Read) - ما منرجّع الـ Entity مباشرة، منرجع View Model
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public int EnrolledStudentsCount { get; set; }
    }

    // اللي المستخدم بيبعته لما يعمل POST (Create)
    public class CourseCreateDto
    {
        [Required(ErrorMessage = "اسم الكورس مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 12, ErrorMessage = "عدد الساعات المعتمدة لازم يكون بين 1 و 12")]
        public int CreditHours { get; set; }

        public int? TeacherId { get; set; }
    }

    // اللي المستخدم بيبعته لما يعمل PUT (Update)
    public class CourseUpdateDto
    {
        [Required(ErrorMessage = "اسم الكورس مطلوب")]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 12)]
        public int CreditHours { get; set; }

        public int? TeacherId { get; set; }
    }
}
