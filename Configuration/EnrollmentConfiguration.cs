using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_3.Configuration;

public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", table =>
        {
            table.HasCheckConstraint(
                "CK_Enrollments_ProgressPercentage_Range",
                "[ProgressPercentage] >= 0 AND [ProgressPercentage] <= 100");
            table.HasCheckConstraint(
                "CK_Enrollments_Status_Matches_Progress",
                "([Status] = N'Completed' AND [ProgressPercentage] = 100) OR ([Status] <> N'Completed' AND [ProgressPercentage] < 100)");
            table.HasCheckConstraint(
                "CK_Enrollments_FinalGrade_OnlyWhenCompleted",
                "[FinalGrade] IS NULL OR ([Status] = N'Completed' AND [FinalGrade] >= 0 AND [FinalGrade] <= 100)");
        });

        builder.HasKey(enrollment => new { enrollment.StudentId, enrollment.CourseId });
        builder.Property(enrollment => enrollment.EnrollmentDate)
            .IsRequired();
        builder.Property(enrollment => enrollment.ProgressPercentage)
            .HasPrecision(5, 2)
            .IsRequired();
        builder.Property(enrollment => enrollment.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(enrollment => enrollment.FinalGrade)
            .HasPrecision(5, 2);

        builder.HasOne(enrollment => enrollment.Student)
            .WithMany(student => student.Enrollments)
            .HasForeignKey(enrollment => enrollment.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(enrollment => enrollment.Course)
            .WithMany(course => course.Enrollments)
            .HasForeignKey(enrollment => enrollment.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
