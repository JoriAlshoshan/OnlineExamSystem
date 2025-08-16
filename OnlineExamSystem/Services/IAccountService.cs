using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Services
{
    public interface IAccountService
    {
        LoginViewModel Login(LoginViewModel vm);
        bool AddTeacher(UserViewModel vm);

        PagedResult<UserViewModel> GetAllTeacher(int pageNumber, int pageSize);
    }
}
