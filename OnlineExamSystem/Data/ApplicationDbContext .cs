using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Data;

public class ApplicationDbContext : IdentityDbContext<UsersApp>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public  DbSet<ExamResult> ExamResults { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Group> Groups { get; set; }    
    public DbSet<QnA> QnA { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<UsersApp> usersApps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsersApp>(entity =>
        {
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);

        }); 
        
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.CVFileName).HasMaxLength(250);
            entity.Property(e => e.PictureFileName).HasMaxLength(250);
            entity.HasOne(d => d.Group).WithMany(P => P.Students).HasForeignKey(d=>d.GroupsId);

        }); 
        modelBuilder.Entity<QnA>(entity =>
        {
            entity.Property(e => e.Question).IsRequired();
            entity.Property(e => e.option1).IsRequired().HasMaxLength(100);
            entity.Property(e => e.option2).IsRequired().HasMaxLength(100);
            entity.Property(e => e.option3).IsRequired().HasMaxLength(100);
            entity.Property(e => e.option4).IsRequired().HasMaxLength(250);
            entity.Property(e => e.Answer).IsRequired();
            entity.HasOne(d => d.Exam).WithMany(P => P.QnA).HasForeignKey(d=>d.ExamId).OnDelete(DeleteBehavior.ClientSetNull);

        });
        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.HasOne(d => d.Users).WithMany(P => P.Groups).HasForeignKey(d=>d.UserSId).OnDelete(DeleteBehavior.ClientSetNull);

        });
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.HasOne(d => d.Group).WithMany(P => P.Exam).HasForeignKey(d => d.GroupId).OnDelete(DeleteBehavior.ClientSetNull);

        }); 
        modelBuilder.Entity<ExamResult>(entity =>
        {
            entity.HasOne(d => d.Exam).WithMany(p => p.ExamResult).HasForeignKey(d => d.ExamId);
            entity.HasOne(d => d.QnA).WithMany(p => p.ExamResult).HasForeignKey(d => d.QnAId).OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.Student).WithMany(p => p.ExamResult).HasForeignKey(d => d.StudentsId).OnDelete(DeleteBehavior.ClientSetNull);

        });
        
        base.OnModelCreating(modelBuilder);
      
      
    }
}
