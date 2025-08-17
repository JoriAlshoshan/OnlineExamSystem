using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineExamApp.Data;
using onlineExamApp.Models;

namespace onlineExamApp.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AvailableExams()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var userUniversity = currentUser.University;
            var now = DateTime.UtcNow;

            var exams = await _db.Exams
                .Include(e => e.Creator)
                .Where(e =>
                    e.IsPublished &&
                    e.StartTimeUtc <= now &&
                    e.EndTimeUtc >= now &&
                    e.Creator != null &&
                    e.Creator.University == userUniversity)
                .ToListAsync();

            return View(exams);
        }


        public async Task<IActionResult> MyResults()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var attempts = await _db.StudentExamAttempts
                .Include(a => a.Exam)
                .Where(a => a.StudentId == userId && a.SubmittedTimeUtc != null)
                .ToListAsync();

            return View(attempts);
        }
    }
}

