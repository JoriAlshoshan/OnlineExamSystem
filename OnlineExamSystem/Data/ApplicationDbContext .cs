using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exam> Exams { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Subject> Subjects { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exam>().ToTable("Exams", "exam");
        modelBuilder.Entity<Question>().ToTable("Questions", "exam");
        modelBuilder.Entity<Option>().ToTable("Options", "exam");
        modelBuilder.Entity<Subject>().ToTable("Subjects", "exam"); 
    }
}
