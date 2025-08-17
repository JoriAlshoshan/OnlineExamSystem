using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using onlineExamApp.Models;

namespace onlineExamApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentExamAttempt> StudentExamAttempts { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Question>()
                .Property(q => q.Points)
                .HasPrecision(10, 2);

            builder.Entity<StudentExamAttempt>()
                .Property(s => s.Score)
                .HasPrecision(10, 2);

            builder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<StudentExamAttempt>()
                .HasMany(a => a.Answers)
                .WithOne(s => s.Attempt)
                .HasForeignKey(s => s.AttemptId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<StudentExamAttempt>()
                .HasOne(a => a.Exam)
                .WithMany()
                .HasForeignKey(a => a.ExamId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<StudentAnswer>()
                .HasOne(sa => sa.SelectedOption)
                .WithMany()
                .HasForeignKey(sa => sa.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
