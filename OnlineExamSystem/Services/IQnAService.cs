using OnlineExamSystem.Models;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Services
{
    public interface IQnAService
    {
        PagedResult<QnAViewModel> GetAll(int pageNumber, int pageSize);
        Task<QnAViewModel> AddAsync(QnAViewModel QnAVM);
        IEnumerable<QnAViewModel> GetAllQnAByExam(int examId);
        bool IsExamAttended (int examId, int studentId);
    }
}
