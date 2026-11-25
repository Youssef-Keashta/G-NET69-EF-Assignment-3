using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_3.Configuration;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", table =>
        {
            table.HasCheckConstraint("CK_Courses_Price_NonNegative", "[Price] >= 0");
            table.HasCheckConstraint(
                "CK_Courses_PublishedDate_Matches_IsPublished",
                "([IsPublished] = 1 AND [PublishedDate] IS NOT NULL) OR ([IsPublished] = 0 AND [PublishedDate] IS NULL)");
        });

        builder.HasKey(course => course.Id);
        builder.Property(course => course.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(course => course.Description)
            .HasMaxLength(2000);
        builder.Property(course => course.Price)
            .HasPrecision(10, 2)
            .IsRequired();
        builder.Property(course => course.Level)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(course => course.IsPublished)
            .IsRequired();
        builder.Property(course => course.PublishedDate);

        builder.HasOne(course => course.Instructor)
            .WithMany(instructor => instructor.Courses)
            .HasForeignKey(course => course.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(course => course.Category)
            .WithMany(category => category.Courses)
            .HasForeignKey(course => course.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
