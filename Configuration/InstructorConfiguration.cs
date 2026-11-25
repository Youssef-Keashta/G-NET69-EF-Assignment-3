using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace G_NET69_EF_Assignment_3.Configuration;

public sealed class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("Instructors");

        builder.HasKey(instructor => instructor.Id);
        builder.Property(instructor => instructor.FullName)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(instructor => instructor.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(instructor => instructor.Email).IsUnique();
        builder.Property(instructor => instructor.Bio)
            .HasMaxLength(1000);
        builder.Property(instructor => instructor.JoinDate)
            .IsRequired();

        builder.HasMany(instructor => instructor.Courses)
            .WithOne(course => course.Instructor)
            .HasForeignKey(course => course.InstructorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
