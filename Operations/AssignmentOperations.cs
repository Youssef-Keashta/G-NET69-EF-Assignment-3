using G_NET69_EF_Assignment_3.DbContexts;
using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_3.Operations;

public sealed class AssignmentOperations
{
    private readonly LearningPlatformDbContext _context;

    public AssignmentOperations(LearningPlatformDbContext context)
    {
        _context = context;
    }

    public int AddInstructor()
    {
        const string email = "demo.instructor@online-learning.example";
        var instructor = _context.Instructors.FirstOrDefault(item => item.Email == email);

        if (instructor is not null)
        {
            Console.WriteLine($"Instructor already exists (Id: {instructor.Id}).");
            return instructor.Id;
        }

        instructor = new Instructor
        {
            FullName = "Demo Instructor",
            Email = email,
            Bio = "An instructor for the assignment demo.",
            JoinDate = DateTime.UtcNow
        };

        _context.Instructors.Add(instructor);
        _context.SaveChanges();
        Console.WriteLine($"Instructor added (Id: {instructor.Id}).");
        return instructor.Id;
    }

    public Course AddCourse(int instructorId, int categoryId)
    {
        var course = new Course
        {
            Title = "EF Core Assignment Demo",
            Description = "A sample course for the assignment demo.",
            Price = 39.99m,
            Level = CourseLevel.Beginner,
            IsPublished = false,
            PublishedDate = null,
            InstructorId = instructorId,
            CategoryId = categoryId
        };

        _context.Courses.Add(course);
        _context.SaveChanges();
        Console.WriteLine($"Course added (Id: {course.Id}).");
        return course;
    }

    public List<Student> AddStudents()
    {
        var tag = Guid.NewGuid().ToString("N");
        var students = new List<Student>
        {
            new() { FullName = "Grace Hopper", Email = $"grace.{tag}@students.example", DateOfBirth = new DateTime(1988, 12, 9) },
            new() { FullName = "Katherine Johnson", Email = $"katherine.{tag}@students.example", DateOfBirth = new DateTime(1990, 8, 26) },
            new() { FullName = "Margaret Hamilton", Email = $"margaret.{tag}@students.example", DateOfBirth = new DateTime(1992, 8, 17) }
        };

        _context.Students.AddRange(students);
        _context.SaveChanges();
        Console.WriteLine("Three students added.");
        return students;
    }

    public void EnrollStudent(int studentId, int courseId)
    {
        var course = _context.Courses.Find(courseId);
        if (course is null || !course.IsPublished)
        {
            Console.WriteLine("The student can only enroll in a published course.");
            return;
        }

        var alreadyEnrolled = _context.Enrollments.Any(item =>
            item.StudentId == studentId && item.CourseId == courseId);
        if (alreadyEnrolled)
        {
            Console.WriteLine("The student is already enrolled in this course.");
            return;
        }

        _context.Enrollments.Add(new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.UtcNow,
            ProgressPercentage = 0m,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        });

        _context.SaveChanges();
        Console.WriteLine("Student enrolled.");
    }

    public void FindCourse(int courseId, string title)
    {
        var courseById = _context.Courses.Find(courseId);
        var courseByTitle = _context.Courses.FirstOrDefault(item => item.Title == title);

        Console.WriteLine(courseById is null
            ? $"Course {courseId} was not found."
            : $"Found by Id: {courseById.Title}");
        Console.WriteLine(courseByTitle is null
            ? $"Course '{title}' was not found."
            : $"Found by title: {courseByTitle.Title} (Id: {courseByTitle.Id})");
    }

    public void FilterCourses(decimal minimumPrice, decimal maximumPrice)
    {
        var courses = _context.Courses
            .Where(course => course.Price >= minimumPrice && course.Price <= maximumPrice)
            .OrderByDescending(course => course.Price)
            .ToList();

        Console.WriteLine($"Courses from {minimumPrice:C} to {maximumPrice:C}:");
        foreach (var course in courses)
        {
            Console.WriteLine($"{course.Title} - {course.Price:C}");
        }
    }

    public void PrintSubCategories(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category is null)
        {
            Console.WriteLine($"Category {categoryId} was not found.");
            return;
        }

        Console.WriteLine($"Sub-categories of {category.Name}:");
        var subCategories = _context.Categories
            .Where(item => item.ParentCategoryId == categoryId)
            .ToList();
        foreach (var subCategory in subCategories)
        {
            Console.WriteLine(subCategory.Name);
        }

        Console.WriteLine("Top-level categories:");
        var topLevelCategories = _context.Categories
            .Where(item => item.ParentCategoryId == null)
            .ToList();
        foreach (var item in topLevelCategories)
        {
            Console.WriteLine(item.Name);
        }
    }

    public void PrintCourseRoster(int courseId)
    {
        var course = _context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine($"Course {courseId} was not found.");
            return;
        }

        var studentIds = _context.Enrollments
            .Where(item => item.CourseId == courseId)
            .Select(item => item.StudentId)
            .ToList();

        var students = _context.Students
            .Where(student => studentIds.Contains(student.Id))
            .OrderBy(student => student.FullName)
            .ToList();

        Console.WriteLine($"Students enrolled in {course.Title}:");
        foreach (var student in students)
        {
            Console.WriteLine(student.FullName);
        }
    }

    public void ChangeCoursePrice(int courseId, decimal newPrice)
    {
        var course = _context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine($"Course {courseId} was not found.");
            return;
        }

        course.Price = newPrice;
        _context.SaveChanges();
        Console.WriteLine("Course price updated.");
    }

    public void PublishCourse(int courseId)
    {
        var course = _context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine($"Course {courseId} was not found.");
            return;
        }

        course.IsPublished = true;
        course.PublishedDate = DateTime.UtcNow;
        _context.SaveChanges();
        Console.WriteLine("Course published.");
    }

    public void UpdateEnrollmentProgress(int studentId, int courseId, decimal progress)
    {
        var enrollment = _context.Enrollments.Find(studentId, courseId);
        if (enrollment is null)
        {
            Console.WriteLine("Enrollment was not found.");
            return;
        }

        enrollment.ProgressPercentage = progress;
        enrollment.Status = progress == 100m ? EnrollmentStatus.Completed : EnrollmentStatus.Active;
        if (enrollment.Status != EnrollmentStatus.Completed)
        {
            enrollment.FinalGrade = null;
        }

        _context.SaveChanges();
        Console.WriteLine("Enrollment progress updated.");
    }

    public void UnenrollStudent(int studentId, int courseId)
    {
        var enrollment = _context.Enrollments.Find(studentId, courseId);
        if (enrollment is null)
        {
            Console.WriteLine("Enrollment was not found.");
            return;
        }

        _context.Enrollments.Remove(enrollment);
        _context.SaveChanges();
        Console.WriteLine("Student unenrolled.");
    }

    public void TryDeleteCategoryWithSubCategories(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category is null)
        {
            Console.WriteLine("Category was not found.");
            return;
        }

        var hasSubCategories = _context.Categories.Any(item => item.ParentCategoryId == categoryId);
        var hasCourses = _context.Courses.Any(item => item.CategoryId == categoryId);
        if (hasSubCategories || hasCourses)
        {
            Console.WriteLine("The category cannot be deleted while it has sub-categories or courses.");
            return;
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
        Console.WriteLine("Category deleted.");
    }

    public void DeleteCategorySafely(int categoryId)
    {
        var category = _context.Categories.Find(categoryId);
        if (category is null)
        {
            Console.WriteLine("Category was not found.");
            return;
        }

        var hasSubCategories = _context.Categories.Any(item => item.ParentCategoryId == categoryId);
        var hasCourses = _context.Courses.Any(item => item.CategoryId == categoryId);
        if (hasSubCategories || hasCourses)
        {
            Console.WriteLine("Category still has sub-categories or courses and cannot be deleted.");
            return;
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
        Console.WriteLine("Category deleted.");
    }

    public void DeleteCourseAndReportEnrollments(int courseId)
    {
        var course = _context.Courses.Find(courseId);
        if (course is null)
        {
            Console.WriteLine($"Course {courseId} was not found.");
            return;
        }

        var enrollmentsBefore = _context.Enrollments.Count(item => item.CourseId == courseId);
        _context.Courses.Remove(course);
        _context.SaveChanges();
        var enrollmentsAfter = _context.Enrollments.Count(item => item.CourseId == courseId);

        Console.WriteLine($"Enrollments before delete: {enrollmentsBefore}; after delete: {enrollmentsAfter}.");
    }
}
