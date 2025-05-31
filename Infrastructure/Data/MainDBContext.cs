using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class MainDbContext : IdentityDbContext<User>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Assignment>()
                .Property(a => a.Status)
                .HasConversion<byte>();

            modelBuilder
                .Entity<Quiz>()
                .Property(q => q.Status)
                .HasConversion<byte>();

            modelBuilder
                .Entity<Notification>()
                .Property(n => n.Type)
                .HasConversion<byte>();

            modelBuilder.Entity<Material>()
                .Property(m => m.FileUrl)
                .HasMaxLength(500);
            modelBuilder.Entity<StudentCourse>()
      .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
              .HasOne(sc => sc.Student)
              .WithMany(u => u.StudentCourses)
              .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
              .HasOne(sc => sc.Course)
              .WithMany(c => c.StudentCourses)
              .HasForeignKey(sc => sc.CourseId);
            modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId);
           
            modelBuilder.Entity<GroupStudent>()
           .HasKey(gs => new { gs.GroupId, gs.StudentId });
            modelBuilder.Entity<Group>()
           .HasOne(g => g.Course)
           .WithMany(c => c.Groups)
           .HasForeignKey(g => g.CourseId);
            modelBuilder.Entity<GroupStudent>()
            .HasOne(gs => gs.Student)
            .WithMany(s => s.GroupStudents)
            .HasForeignKey(gs => gs.StudentId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Instructor)
                .WithMany()
                .HasForeignKey(g => g.InstructorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
