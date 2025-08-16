using OnlineExamSystem.Models;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Services
{
    public interface IExamService
    {
        PagedResult<ExamViewModel>GetAll(int pageNumber, int pageSize);
        Task<ExamViewModel> AddAsync(ExamViewModel examVM);
        IEnumerable<Exam> GetAllExam();
    }
}
