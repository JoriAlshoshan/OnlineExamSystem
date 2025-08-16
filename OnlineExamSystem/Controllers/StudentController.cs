using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Services;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Controllers
{
   
    public class StudentController : Controller
    {

        private readonly IStudentService _studentService;
        private readonly IExamService _examService;
        private readonly IQnAService _qnAService;

        public StudentController(IStudentService studentService, IExamService examService, IQnAService qnAService)
        {
            _studentService = studentService;
            _examService = examService;
            _qnAService = qnAService;
        }

        public IActionResult Index(int pageNumber=1,int pageSize=10)
        {
            return View(_studentService.GetAll(pageNumber,pageSize));
        }
        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddAsync(studentViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(studentViewModel);
        }
        [Authorize(Roles = "Student")]

        public IActionResult AttendExam() 
        {
            var model = new AttendExamViewModel();
            LoginViewModel sessionObj = HttpContext.
                Session.Get<LoginViewModel>("loginvm");
            if(sessionObj!= null)
            {
                model.StudentId = Convert.ToInt32(sessionObj.Id);
                model.QnA = new List<QnAViewModel>();
                var todayExam = _examService.GetAllExam().
                    Where(a=>a.StartDate.Date==DateTime.Today.Date).FirstOrDefault();
                if (todayExam == null)
                {
                    model.Message = "No Exam Scheduled Today";
                }
                else
                {
                    if(!_qnAService.IsExamAttended(todayExam.Id,model.StudentId))
                    {
                        model.QnA= _qnAService.GetAllQnAByExam(todayExam.Id).ToList();
                        model.ExamName = todayExam.Title;
                        model.Message = "";
                    }
                    else
                    
                        model.Message = "You Have Already Attend This Exam";
                    
                }
                return View(model);

            }
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public ActionResult AttendExam(AttendExamViewModel attendExamViewModel)
        {
            bool result = _studentService.SetExamResult(attendExamViewModel);
            return RedirectToAction("AttendExam");
        }

        //[Authorize(Roles = "Teacher")]

        public IActionResult Result(string studentId)
        {
            var model = _studentService.GetExamResults(Convert.ToInt32(studentId)); 
            return View(model);

        }

        [Authorize(Roles = "Student")]
        public IActionResult ViewResult()
        {
            LoginViewModel seesionObj = HttpContext.Session.Get<LoginViewModel>("loginvm");
            if (seesionObj != null)
            {
                var model = _studentService.GetExamResults(Convert.ToInt32(seesionObj.Id));
                return View(model);
            }
            return RedirectToAction("Login","Account");
        }
    }
}
