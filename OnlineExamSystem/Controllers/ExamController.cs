using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Services;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamService _examservice;
        private readonly IGroupService _groupservice;

        public ExamController(IExamService examservice, IGroupService groupservice)
        {
            _examservice = examservice;
            _groupservice = groupservice;
        }

        [Authorize(Roles = "Educator")]

        public IActionResult Index(int pageNumber =1,int pageSize=10)
        {
            return View(_examservice.GetAll(pageNumber,pageSize));
        }
        [Authorize(Roles = "Educator")]

        public IActionResult Create(int id)
        {
            var model = new ExamViewModel();
            model.GroupList = (IEnumerable<Models.Group>)_groupservice.GetAllGroups();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            { 
                await _examservice.AddAsync(examViewModel);
                return RedirectToAction(nameof(Index));   
            }
           
            return View(examViewModel);
        }


    }
}
