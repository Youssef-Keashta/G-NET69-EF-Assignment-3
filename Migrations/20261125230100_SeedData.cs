using G_NET69_EF_Assignment_3.DbContexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G_NET69_EF_Assignment_3.Migrations;

public partial class SeedData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Instructors",
            columns: new[] { "Id", "FullName", "Email", "Bio", "JoinDate" },
            values: new object[,]
            {
                { 1, "Ada Lovelace", "ada.lovelace@learning.example", "Mathematician and early computing pioneer.", new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
                { 2, "Alan Turing", "alan.turing@learning.example", "Computer scientist and cryptanalyst.", new DateTime(2021, 3, 10, 0, 0, 0, DateTimeKind.Utc) }
            });

        migrationBuilder.InsertData(
            table: "Categories",
            columns: new[] { "Id", "Name", "Description", "ParentCategoryId" },
            values: new object[,]
            {
                { 1, "Programming", "Software development courses.", null },
                { 4, "Data Science", "Data analysis and machine learning.", null },
                { 5, "Design", "Design and user-experience courses.", null }
            });
        migrationBuilder.InsertData(
            table: "Categories",
            columns: new[] { "Id", "Name", "Description", "ParentCategoryId" },
            values: new object[,]
            {
                { 2, "Backend", "Server-side programming.", 1 },
                { 3, "ASP.NET Core", "Web development with ASP.NET Core.", 2 }
            });

        migrationBuilder.InsertData(
            table: "Students",
            columns: new[] { "Id", "FullName", "Email", "DateOfBirth" },
            values: new object[,]
            {
                { 1, "Grace Hopper", "grace.hopper@students.example", new DateTime(1988, 12, 9, 0, 0, 0, DateTimeKind.Utc) },
                { 2, "Katherine Johnson", "katherine.johnson@students.example", new DateTime(1990, 8, 26, 0, 0, 0, DateTimeKind.Utc) },
                { 3, "Margaret Hamilton", "margaret.hamilton@students.example", new DateTime(1992, 8, 17, 0, 0, 0, DateTimeKind.Utc) }
            });

        migrationBuilder.InsertData(
            table: "Courses",
            columns: new[] { "Id", "Title", "Description", "Price", "Level", "IsPublished", "PublishedDate", "InstructorId", "CategoryId" },
            values: new object[,]
            {
                { 1, "C# Basics", "An introduction to the C# language.", 39.99m, "Beginner", true, new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                { 2, "ASP.NET Core Fundamentals", "Build web applications with ASP.NET Core.", 79.99m, "Intermediate", true, new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc), 2, 2 },
                { 3, "Advanced Data Structures", "A deeper look at common data structures.", 59.99m, "Advanced", true, new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                { 4, "Machine Learning Preview", "A course that has not yet been published.", 99.99m, "Advanced", false, null, 2, 4 }
            });

        migrationBuilder.InsertData(
            table: "Enrollments",
            columns: new[] { "StudentId", "CourseId", "EnrollmentDate", "ProgressPercentage", "Status", "FinalGrade" },
            values: new object[,]
            {
                { 1, 1, new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), 100m, "Completed", 95.50m },
                { 2, 1, new DateTime(2024, 1, 18, 0, 0, 0, DateTimeKind.Utc), 45m, "Active", null },
                { 3, 2, new DateTime(2024, 2, 25, 0, 0, 0, DateTimeKind.Utc), 20m, "Active", null }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Enrollments",
            keyColumns: new[] { "StudentId", "CourseId" },
            keyValues: new object[,] { { 1, 1 }, { 2, 1 }, { 3, 2 } });

        migrationBuilder.DeleteData(table: "Courses", keyColumn: "Id", keyValues: new object[] { 1, 2, 3, 4 });
        migrationBuilder.DeleteData(table: "Students", keyColumn: "Id", keyValues: new object[] { 1, 2, 3 });
        migrationBuilder.DeleteData(table: "Categories", keyColumn: "Id", keyValues: new object[] { 3, 2, 1, 4, 5 });
        migrationBuilder.DeleteData(table: "Instructors", keyColumn: "Id", keyValues: new object[] { 1, 2 });
    }
}
