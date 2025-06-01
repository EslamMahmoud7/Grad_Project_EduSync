// Infrastructure/Data/MainDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using MediatR;

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
        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }
        public DbSet<AcademicRecord> AcademicRecords { get; set; }
        public DbSet<QuizModel> QuizModels { get; set; } = default!;
        public DbSet<Question> Questions { get; set; } = default!;
        public DbSet<QuestionOption> QuestionOptions { get; set; } = default!;
        public DbSet<StudentQuizAttempt> StudentQuizAttempts { get; set; } = default!;
        public DbSet<StudentQuizAnswer> StudentQuizAnswers { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Material>()
                .Property(m => m.FileUrl)
                .HasMaxLength(500);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Group)
                .WithMany(g => g.Assignments)
                .HasForeignKey(a => a.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Group)
                .WithMany(g => g.Quizzes)
                .HasForeignKey(q => q.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.Group)
                .WithMany(g => g.Materials)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Course)
                .WithMany(c => c.Groups)
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupStudent>()
                .HasKey(gs => new { gs.GroupId, gs.StudentId });

            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Student)
                .WithMany(s => s.GroupStudents)
                .HasForeignKey(gs => gs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Group)
                .WithMany(g => g.GroupStudents)
                .HasForeignKey(gs => gs.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Instructor)
                .WithMany(i => i.Groups)
                .HasForeignKey(g => g.InstructorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<AcademicRecord>()
                .Property(ar => ar.Status)
                .HasConversion<byte>();

            modelBuilder.Entity<AcademicRecord>()
                .Property(ar => ar.AssessmentType)
                .HasConversion<byte>();
            modelBuilder.Entity<AcademicRecord>()
                .HasOne(ar => ar.Student)
                .WithMany()
                .HasForeignKey(ar => ar.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AcademicRecord>()
                .HasOne(ar => ar.Group)
                .WithMany()
                .HasForeignKey(ar => ar.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AcademicRecord>()
                .HasOne(ar => ar.Instructor)
                .WithMany()
                .HasForeignKey(ar => ar.InstructorId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasOne(q => q.Group)
                    .WithMany(g => g.Quizzes)
                    .HasForeignKey(q => q.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(q => q.Instructor)
                    .WithMany()
                    .HasForeignKey(q => q.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<QuizModel>(entity =>
            {
                entity.HasOne(qm => qm.Quiz)
                    .WithMany(q => q.QuizModels)
                    .HasForeignKey(qm => qm.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.QuizModel)
                    .WithMany(qm => qm.Questions)
                    .HasForeignKey(q => q.QuizModelId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(q => q.Type)
                    .HasConversion<byte>();
            });

            modelBuilder.Entity<QuestionOption>(entity =>
            {
                entity.HasOne(qo => qo.Question)
                    .WithMany(q => q.Options)
                    .HasForeignKey(qo => qo.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<StudentQuizAttempt>(entity =>
            {
                entity.HasOne(sqa => sqa.Student)
                    .WithMany()
                    .HasForeignKey(sqa => sqa.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sqa => sqa.Quiz)
                    .WithMany(q => q.Attempts)
                    .HasForeignKey(sqa => sqa.QuizId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sqa => sqa.QuizModel)
                    .WithMany(qm => qm.AttemptsForThisModel)
                    .HasForeignKey(sqa => sqa.QuizModelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(sqa => sqa.Status)
                    .HasConversion<byte>();
            });

            modelBuilder.Entity<StudentQuizAnswer>(entity =>
            {
                entity.HasOne(sqans => sqans.StudentQuizAttempt)
                    .WithMany(sqa => sqa.Answers)
                    .HasForeignKey(sqans => sqans.StudentQuizAttemptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sqans => sqans.Question)
                    .WithMany(q => q.StudentAnswers)
                    .HasForeignKey(sqans => sqans.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sqans => sqans.SelectedOption)
                    .WithMany()
                    .HasForeignKey(sqans => sqans.SelectedOptionId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}