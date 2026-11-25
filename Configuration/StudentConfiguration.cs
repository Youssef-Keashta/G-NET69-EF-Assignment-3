using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_3.Configuration;

public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(student => student.Id);
        builder.Property(student => student.FullName)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(student => student.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(student => student.Email).IsUnique();
        builder.Property(student => student.DateOfBirth)
            .IsRequired();

        builder.HasMany(student => student.Enrollments)
            .WithOne(enrollment => enrollment.Student)
            .HasForeignKey(enrollment => enrollment.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
