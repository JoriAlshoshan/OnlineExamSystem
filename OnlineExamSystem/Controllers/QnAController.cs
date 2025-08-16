using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Services;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Controllers
{
    public class QnAController: Controller
    {
        private readonly IExamService _examService;
        private readonly IQnAService _qnAService;

        public QnAController(IExamService examService, IQnAService qnAService)
        {
            _examService = examService;
            _qnAService = qnAService;
        }
        [Authorize(Roles = "Educator")]

        public ActionResult Index(int pageNumber=1, int pageSize=10)
        {
            return View(_qnAService.GetAll(pageNumber,pageSize));
        }
        [Authorize(Roles = "Educator")]

        public ActionResult Create()
        {
            var model = new QnAViewModel();
            model.EXamList = _examService.GetAllExam();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QnAViewModel qnAViewModel)
        {
            if(ModelState.IsValid)
            {
                await _qnAService.AddAsync(qnAViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(qnAViewModel);
        }
    }
}
