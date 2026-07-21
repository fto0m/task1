# Course Management API (ASP.NET Core)

مشروع Web API بسيط بـ ASP.NET Core 8 يمثّل نظام Courses / Students / Teachers مع علاقات بينهم، CRUD كامل، Validation، Swagger، و integration مع قاعدة بيانات حقيقية (SQLite) عن طريق Entity Framework Core.

## المتطلبات
- .NET 10 SDK: https://dotnet.microsoft.com/download

## طريقة التشغيل
```bash
cd CourseManagementApi
dotnet restore
dotnet run
```
بعدها افتح المتصفح على الرابط اللي بيطلع بالـ terminal (أو مباشرة `http://localhost:5080/swagger`) وبيفتحلك Swagger UI فيه كل الـ endpoints جاهزة تجربها.

أول تشغيل، البرنامج بينشئ ملف `courses.db` (SQLite) تلقائيًا وبيحط فيه بيانات تجريبية (Seed).

## هيكلية المشروع
```
CourseManagementApi/
 ├─ Models/              # الـ Entities: Course, Student, Teacher, StudentCourse
 ├─ DTOs/                # الـ View Models (Read / Create / Update) لكل Entity
 ├─ Data/AppDbContext.cs # EF Core DbContext + العلاقات + Seed data
 ├─ Controllers/         # CoursesController, StudentsController, TeachersController
 └─ Program.cs           # إعداد Swagger, EF Core, Middleware
```

## العلاقات
- **Teacher → Course**: One-to-Many (مدرّس واحد بيدرّس أكتر من كورس).
- **Student ↔ Course**: Many-to-Many عن طريق جدول الوصل `StudentCourse`.

## أهم الـ Endpoints
| Method | Route | الوظيفة |
|---|---|---|
| GET | /api/courses | جلب كل الكورسات |
| GET | /api/courses/{id} | جلب كورس معيّن |
| POST | /api/courses | إضافة كورس جديد |
| PUT | /api/courses/{id} | تعديل كورس |
| DELETE | /api/courses/{id} | حذف كورس |
| GET/POST/PUT/DELETE | /api/students(/{id}) | نفس الشي للطلاب |
| GET/POST/PUT/DELETE | /api/teachers(/{id}) | نفس الشي للمدرّسين |
| **POST** | **/api/students/{studentId}/courses/{courseId}** | **تسجيل طالب بكورس (enroll)** |
| DELETE | /api/students/{studentId}/courses/{courseId} | إلغاء تسجيل طالب من كورس |

## التبديل بين In-Memory و DB حقيقية
جوا `Program.cs` في سطرين: واحد مفعّل (`UseSqlite`) وواحد معلّق (`UseInMemoryDatabase`). لو بدك تجرب بدون ملف db فعلي، علّق سطر الـ SQLite وفعّل سطر الـ InMemory.

