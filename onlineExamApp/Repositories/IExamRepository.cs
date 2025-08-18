using onlineExamApp.Models;

namespace onlineExamApp.Repositories
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<Exam?> GetExamWithQuestionsAsync(int examId);
        Task<IEnumerable<Exam>> GetAvailableExamsForStudentAsync(string studentUniversity);
    }
}
