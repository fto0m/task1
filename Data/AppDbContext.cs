using CourseManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // مفتاح مركّب (Composite Key) لجدول الوصل
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            // بيانات تجريبية (Seed) عشان يشتغل الـ Swagger فيه شي جاهز نجربه
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, FullName = "Dr. Ahmad Odeh", Email = "ahmad.odeh@example.com" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Intro to Programming", Description = "أساسيات البرمجة", CreditHours = 3, TeacherId = 1 }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FullName = "Fatima", Email = "fatima@example.com" }
            );
        }
    }
}
