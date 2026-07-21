namespace CourseManagementApi.Models
{
    // جدول الوصل اللي بيمثل تسجيل الطالب بكورس معين (العلاقة Many-to-Many)
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
