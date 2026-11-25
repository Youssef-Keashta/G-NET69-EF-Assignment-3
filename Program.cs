using G_NET69_EF_Assignment_3.DbContexts;
using G_NET69_EF_Assignment_3.Operations;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_3;

public static class Program
{
    public static void Main(string[] args)
    {
        using var context = new LearningPlatformDbContext();
        context.Database.Migrate();

        var operations = new AssignmentOperations(context);

        int instructorId = operations.AddInstructor();
        var course = operations.AddCourse(instructorId, 1);
        var students = operations.AddStudents();

        operations.EnrollStudent(students[0].Id, 2);
        operations.FindCourse(2, "ASP.NET Core Fundamentals");
        operations.FilterCourses(20m, 100m);
        operations.PrintSubCategories(1);
        operations.PrintCourseRoster(2);

        operations.ChangeCoursePrice(course.Id, 49.99m);
        operations.PublishCourse(course.Id);
        operations.UpdateEnrollmentProgress(students[0].Id, 2, 100m);
        operations.UnenrollStudent(students[0].Id, 2);

        operations.TryDeleteCategoryWithSubCategories(1);
        operations.DeleteCategorySafely(1);
        operations.DeleteCourseAndReportEnrollments(4);
    }
}
