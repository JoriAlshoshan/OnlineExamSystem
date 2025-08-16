using OnlineExamSystem.Models;
using OnlineExamSystem.UnitOfWork;
using OnlineExamSystem.ViewModels;
using System.Collections;
using System.Linq;

namespace OnlineExamSystem.Services
{
    public class StudentService: IStudentService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _iLogger;

        public StudentService(IUnitOfWork unitOfWork,ILogger<StudentService> iLogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = iLogger;
        }

        public async Task<StudentViewModel> AddAsync(StudentViewModel vm)
        {
            try
            {
                Student obj = vm.ConvertViewModel(vm);
                await _unitOfWork.GenericRepository<Student>().AddAsync(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
            return vm;  
        }

        public PagedResult<StudentViewModel> GetAll(int pagedNumber, int pageSize)
        {
            var model = new StudentViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pagedNumber) - pageSize;
                List<StudentViewModel> detailsList = new List<StudentViewModel>();
                var modelList = _unitOfWork.GenericRepository<Student>().GetAll().
                    Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Student>().GetAll().ToList();
                detailsList = GroupListInfo(modelList);
                if (detailsList != null)
                {
                    model.StudentList = detailsList;
                    model.TotalCount = totalCount.Count();
                }

            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<StudentViewModel>
            {
                Data = model.StudentList,
                TotalItem = model.TotalCount,
                PageNumber = pagedNumber,
                PageSize = pageSize
            };
            return result;
        }

        private List<StudentViewModel> GroupListInfo(List<Student> modelList)
        {
            return modelList.Select(o => new StudentViewModel(o)).ToList();
        }

        public IEnumerable<Student> GetAlStudents()
        {
            try
            {
                var student = _unitOfWork.GenericRepository<Student>().GetAll(); 
                return student; 
            }
            catch (Exception ex) 
            {
                _iLogger.LogError(ex.Message);
            }
            return Enumerable.Empty<Student>();
        }

        public IEnumerable<ExamResultViewModel> GetExamResults(int studentId)
        {
            try
            {
                var examResult = _unitOfWork.GenericRepository<ExamResult>().GetAll()
                    .Where(a => a.StudentsId == studentId); 
                var students =_unitOfWork.GenericRepository<Student>().GetAll();
                var exam = _unitOfWork.GenericRepository<ExamResult>().GetAll();
                var qna = _unitOfWork.GenericRepository<QnA>().GetAll();

                var requiredData = examResult.Join(students, er => er.StudentsId, s => s.Id,
                    (er, st) => new { er, st }).Join(exam, erj => erj.er.ExamId, ex => ex.Id,
                    (erj, ex) => new { erj, ex }).Join(qna, exj => exj.erj.er.QnAId, q => q.Id,
                    (exj, q) => new ExamResultViewModel()
                    {
                        StudentId = studentId,  
                        ExamName= exj.ex.Title,
                        TotalQuetion = examResult.Count(a=>a.StudentsId==studentId
                        && a.ExamId==exj.ex.Id), CorrectAnswer= examResult.Count(a=>a.StudentsId==studentId&&
                        a.ExamId!=exj.ex.Id && a.Answer==q.Answer),
                        WrongAnswer = examResult.Count(a=>a.StudentsId==studentId && 
                        a.ExamId==exj.ex.Id && a.Answer != q.Answer )
                    });
                return requiredData;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return Enumerable.Empty<ExamResultViewModel>();
        }

        public StudentViewModel GetStudentDetails(int studentId)
        {
            try
            {
                var student = _unitOfWork.GenericRepository<Student>().GetByID(studentId);
                return student != null ? new StudentViewModel(student) : null;
            }
            catch (Exception ex)
            {   
                _iLogger.LogError(ex.Message);  
            }
            return null;
        }

        public bool SetExamResult(AttendExamViewModel vm)
        {
            try
            {
                foreach (var item in vm.QnA)
                {
                    ExamResult examResult = new ExamResult();
                    examResult.StudentsId = vm.StudentId;
                    examResult.QnAId = item.Id;
                    examResult.ExamId = item.ExamId;
                    examResult.Answer = item.SelectedAnswar;
                    _unitOfWork.GenericRepository<ExamResult>().AddAsync(examResult);
                }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return false;
        }

        public bool SetGroupIdToStudents(GroupViewModel vm)
        {
            try
            {
                foreach (var item in vm.StudentCheckList)
                {
                    var student = _unitOfWork.GenericRepository<Student>().GetByID(item.Id);
                    if (item.Selected)
                    {
                        student.GroupsId = vm.Id;
                        _unitOfWork.GenericRepository<Student>().Update(student);
                    }
                    else
                    {
                        if (student.GroupsId == vm.Id)
                        {
                            student.GroupsId = null;

                        }
                    }
                    _unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            return false;

         }     

        public async Task<StudentViewModel> UpdateAsync(StudentViewModel vm)
        {
            try
            {
                Student obj = _unitOfWork.GenericRepository<Student>().GetByID(vm.Id);
                obj.Name = vm.Name;
                obj.UserName = vm.UserName; 
                obj.PictureFileName = vm.PictureFileName !=null?
                    vm.PictureFileName :  obj.PictureFileName;
                obj.CVFileName = vm.CVFileName != null?
                    vm.CVFileName : obj.CVFileName; 
                obj.Contact = vm.Contact;
                await _unitOfWork.GenericRepository<Student>().UpdateAsync(obj);
                _unitOfWork.Save();

            }
            catch(Exception ex) 
            {
                throw;
            }
            return vm;
        }
    }
}
