using OnlineExamSystem.Models;
using OnlineExamSystem.UnitOfWork;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Services
{
    public class AccountService : IAccountService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _iLogger;

        public AccountService(IUnitOfWork unitOfWork, ILogger<StudentService> Ilogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = Ilogger;
        }

       
        public bool AddTeacher(UserViewModel vm)
        {
            try
            {
                UsersApp obj = new UsersApp()
                {
                    UserName = vm.UserName,
                    Role = (int)EnumRoles.Teacher
                };
                _unitOfWork.GenericRepository<UsersApp>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
                return false;
            }
            return true;
        }

        public PagedResult<UserViewModel> GetAllTeacher(int pageNumber, int pageSize)
        {
            var model = new UserViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<UserViewModel> detailsList = new List<UserViewModel>();
                var modelList = _unitOfWork.GenericRepository<UsersApp>().GetAll()
                    .Where(x => x.Role == (int)EnumRoles.Teacher).Skip(ExcludeRecords)
                    .Take(pageSize).ToList();
                detailsList = ListInfo(modelList);
                if (detailsList != null)
                {
                    model.UserList = detailsList;
                    model.TotalCount = _unitOfWork.GenericRepository<UsersApp>().GetAll()
                        .Count(x => x.Role == (int)EnumRoles.Teacher);

                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var results = new PagedResult<UserViewModel>
            {
                Data = model.UserList,
                TotalItem = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            return results;
        }

        private List<UserViewModel> ListInfo(List<UsersApp> modelList)
        {
            return modelList.Select(o=> new UserViewModel(o)).ToList();
        }

        public LoginViewModel Login(LoginViewModel vm)
        {
            if (vm.Role == (int)EnumRoles.Admin || vm.Role == (int)EnumRoles.Teacher)
            {
                var user = _unitOfWork.GenericRepository<UsersApp>().GetAll().
                    FirstOrDefault(a => a.UserName == vm.UserName.Trim() 
                    && a.Role == vm.Role);
                if (user != null)
                {
                    //vm.Id = user.Id;
                    return vm;
                }
            }
            else
            {
                var student = _unitOfWork.GenericRepository<Student>().GetAll().
                    FirstOrDefault(a => a.UserName == vm.UserName.Trim()
                    && a.Password == vm.Password.Trim());
                if (student != null)
                {
                    vm.Id = student.Id;
                }
                return vm;
            }
            return null;
        }
    }
}
