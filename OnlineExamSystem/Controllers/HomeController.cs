using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.Models;
using System.Diagnostics;

namespace OnlineExamSystem.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;

        public RoleController(ILogger<RoleController> logger)
        {
            _logger = logger;
        }

        

        

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }

        //[Authorize(Roles = "Educator")]
        //public IActionResult EducatorPage()
        //{
        //    return View();
        //}

        [Authorize(Roles = "Student")]
        public IActionResult StudentPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //here crud Operation 
    }
}
