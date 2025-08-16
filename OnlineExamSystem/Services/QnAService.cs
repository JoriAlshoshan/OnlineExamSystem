using OnlineExamSystem.Models;
using OnlineExamSystem.UnitOfWork;
using OnlineExamSystem.ViewModels;
using System.Linq;

namespace OnlineExamSystem.Services
{
    public class QnAService : IQnAService
    {
        IUnitOfWork _unitOfWork;
        ILogger<QnAService> _iLogger;

        public QnAService(IUnitOfWork unitOfWork, ILogger<QnAService> iLogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = iLogger;
        }

        public async Task<QnAViewModel> AddAsync(QnAViewModel QnAVM)
        {
            try
            {
                QnA objGroup = QnAVM.ConvertViewModel(QnAVM);
                await _unitOfWork.GenericRepository<QnA>().AddAsync(objGroup);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return null;
            }
            return QnAVM;
        }

        public PagedResult<QnAViewModel> GetAll(int pageNumber, int pageSize)
        {
            var model = new QnAViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<QnAViewModel> deatailList = new List<QnAViewModel>();
                var modelList = _unitOfWork.GenericRepository<QnA>().GetAll().Skip(ExcludeRecords).
                    Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<QnA>().GetAll().ToList();
                deatailList = ListInfo(modelList);
                if (deatailList != null)
                {
                    model.qnAList = deatailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<QnAViewModel>
            {
                Data = model.qnAList,
                TotalItem = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
        }

        private List<QnAViewModel> ListInfo(List<QnA> modelList)
        {
            return modelList.Select(o => new QnAViewModel(o)).ToList();
        }

        public IEnumerable<QnAViewModel> GetAllQnAByExam(int examId)
        {
            try
            {
                var qnaList = _unitOfWork.GenericRepository<QnA>().GetAll().
                    Where(x=>x.ExamId==examId);
                return ListInfo(qnaList.ToList());
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);

            }
            return Enumerable.Empty<QnAViewModel>();
        }

        public bool IsExamAttended(int examId, int studentId)
        {
            try
            {
                var qnaRecord =_unitOfWork.GenericRepository<ExamResult>().
                    GetAll().FirstOrDefault(x=>x.ExamId == examId && x.StudentsId==studentId);
                return qnaRecord == null ? false : true;
            }
            catch (Exception ex)
            { 
                _iLogger.LogError(ex.Message);
            }
            return false;
        }
    }
}
