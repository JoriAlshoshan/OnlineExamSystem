using Microsoft.EntityFrameworkCore;
using onlineExamApp.Data;
using onlineExamApp.Models;
using System.Threading.Tasks;

public class StudentExamAttemptRepository : Repository<StudentExamAttempt>, IStudentExamAttemptRepository
{
    private readonly ApplicationDbContext _context;

    public StudentExamAttemptRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<StudentExamAttempt?> GetAttemptWithDetailsAsync(int attemptId)
    {
        return await _context.StudentExamAttempts
            .Include(a => a.Exam)
                .ThenInclude(e => e.Questions)
                    .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(a => a.Id == attemptId);
    }
}
