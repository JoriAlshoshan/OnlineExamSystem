using OnlineExamSystem.Models;
using OnlineExamSystem.UnitOfWork;
using OnlineExamSystem.ViewModels;
using System.Linq;
using System.Text.RegularExpressions;

namespace OnlineExamSystem.Services
{
    public class ExamService : IExamService
    {

        IUnitOfWork _unitOfWork;
        ILogger<ExamService> _iLogger;

        public ExamService(IUnitOfWork unitOfWork, ILogger<ExamService> iLogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = iLogger;
        }

        public async Task<ExamViewModel> AddAsync(ExamViewModel examVM)
        {

            try
            {
                Exam objGroup = examVM.ConvertViewModel(examVM);
                await _unitOfWork.GenericRepository<Exam>().AddAsync(objGroup);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return null;
            }
            return examVM;
        }

        public PagedResult<ExamViewModel> GetAll(int pageNumber, int pageSize)
        {
            var model = new ExamViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<ExamViewModel> deatailList = new List<ExamViewModel>();
                var modelList = _unitOfWork.GenericRepository<Exam>().GetAll().Skip(ExcludeRecords).
                    Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Exam>().GetAll().ToList();
                deatailList = ExamListInfo(modelList);
                if (deatailList != null)
                {
                    model.ExamList = deatailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<ExamViewModel>
            {
                Data = model.ExamList,
                TotalItem = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        private List<ExamViewModel> ExamListInfo(List<Exam> modelList)
        {
            return modelList.Select(o => new ExamViewModel(o)).ToList();

        }

        public IEnumerable<Exam> GetAllExam()
        {
            try
            {
                var exams = _unitOfWork.GenericRepository<Exam>().GetAll();
                return exams;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);

            }
            return Enumerable.Empty<Exam>();
        }
    }
}
