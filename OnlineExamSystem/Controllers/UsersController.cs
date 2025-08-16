using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Services;
using OnlineExamSystem.ViewModels;

namespace OnlineExamSystem.Controllers
{
    //[Authorize(Roles = "Student")]

    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index(int pageNumber =1,int pageSize = 10)
        {
            return View(_accountService.GetAllTeacher(pageNumber,pageSize));
        }

        public IActionResult Create(int id)
        { 
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _accountService.AddTeacher(userViewModel);
                return RedirectToAction("Index");
            }
            return View(userViewModel);

        }
    }
}
