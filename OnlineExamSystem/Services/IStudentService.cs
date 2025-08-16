using OnlineExamSystem.Models;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Services
{
    public interface IStudentService
    {
        PagedResult<StudentViewModel> GetAll(int pagedNumber,int pageSize);
        Task<StudentViewModel>AddAsync(StudentViewModel vm);
        IEnumerable<Student> GetAlStudents();
        bool SetGroupIdToStudents(GroupViewModel vm);
        bool SetExamResult(AttendExamViewModel vm);
        IEnumerable<ExamResultViewModel> GetExamResults(int studentId);
        StudentViewModel GetStudentDetails(int studentId);
        Task<StudentViewModel> UpdateAsync(StudentViewModel vm);
    }
}
