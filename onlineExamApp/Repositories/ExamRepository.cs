using Microsoft.EntityFrameworkCore;
using onlineExamApp.Data;
using onlineExamApp.Models;
using onlineExamApp.Repositories;

public class ExamRepository : Repository<Exam>, IExamRepository
{
    public ExamRepository(ApplicationDbContext db) : base(db) { }

    public async Task<Exam?> GetExamWithQuestionsAsync(int examId)
    {
        return await _dbSet
            .Include(e => e.Questions).ThenInclude(q => q.Options)
            .Include(e => e.Creator)
            .FirstOrDefaultAsync(e => e.Id == examId);
    }

    public async Task<IEnumerable<Exam>> GetAvailableExamsForStudentAsync(string studentUniversity)
    {
        var now = DateTime.UtcNow;

        return await _dbSet
            .Include(e => e.Creator)
            .Where(e =>
                e.IsPublished &&
                e.StartTimeUtc <= now &&
                e.EndTimeUtc >= now &&
                (string.IsNullOrEmpty(studentUniversity) || studentUniversity == "Unknown University"
                    ? true
                    : e.Creator.University == studentUniversity ||
                      string.IsNullOrEmpty(e.Creator.University) ||
                      e.Creator.University == "Unknown University"))
            .ToListAsync();
    }
}
