using onlineExamApp.Models;
using System.Threading.Tasks;

public interface IStudentExamAttemptRepository : IRepository<StudentExamAttempt>
{
    Task<StudentExamAttempt?> GetAttemptWithDetailsAsync(int attemptId);
}
