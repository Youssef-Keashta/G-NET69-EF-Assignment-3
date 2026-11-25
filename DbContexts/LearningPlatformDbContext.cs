using G_NET69_EF_Assignment_3.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET69_EF_Assignment_3.DbContexts
{
    public class LearningPlatformDbContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = DESKTOP-VGTVSKK;" +
                "Database = LearningPlatformDb;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate = True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningPlatformDbContext).Assembly);
        }
    }
}
