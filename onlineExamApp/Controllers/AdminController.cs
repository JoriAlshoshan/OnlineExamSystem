using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineExamApp.Data;
using onlineExamApp.Models;
using onlineExamApp.ViewModel;

namespace onlineExamApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var studentRoleId = _db.Roles.Where(r => r.Name == "Student").Select(r => r.Id).FirstOrDefault();
            int totalStudents = studentRoleId != null ? allUsers.Count(u => _userManager.IsInRoleAsync(u, "Student").Result) : 0;
            var educatorRoleId = _db.Roles.Where(r => r.Name == "Educator").Select(r => r.Id).FirstOrDefault();
            int totalEducators = educatorRoleId != null ? allUsers.Count(u => _userManager.IsInRoleAsync(u, "Educator").Result) : 0;
            int totalUniversities = allUsers.Where(u => !string.IsNullOrEmpty(u.University)).Select(u => u.University).Distinct().Count();
            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalEducators = totalEducators;
            ViewBag.TotalUniversities = totalUniversities;
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var accessibleUsers = await _db.Users
                .Where(u => u.University == currentUser.University)
                .ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in accessibleUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(user.Id, roles);
            }
            ViewData["UserRoles"] = userRoles;
            return View(accessibleUsers);
        }

        [HttpGet]
        public IActionResult CreateUser() => View(new CreateUserViewModel());

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, DisplayName = model.DisplayName, University = model.University };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
                model.AllRoles = new List<string> { "Admin", "Educator", "Student" };
                return View(model);
            }
            if (model.Roles.Any()) await _userManager.AddToRolesAsync(user, model.Roles);
            TempData["Success"] = "User created successfully!";
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            var allRoles = new List<string> { "Admin", "Educator", "Student" };
            var model = new EditUserViewModel { Id = user.Id, DisplayName = user.DisplayName, Email = user.Email, University = user.University, Roles = roles, AllRoles = allRoles };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _db.Users.FindAsync(model.Id);
            if (user == null) return NotFound();
            user.DisplayName = model.DisplayName;
            user.Email = model.Email;
            user.University = model.University;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(model.Roles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            TempData["Success"] = "User updated successfully!";
            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user != null) { _db.Users.Remove(user); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(ManageUsers));
        }

        public IActionResult ExamCompletion()
        {
            var totalExams = _db.Exams.Count();
            var totalStarted = _db.StudentExamAttempts.Count();
            var totalCompleted = _db.StudentExamAttempts.Count(a => a.SubmittedTimeUtc != null);
            ViewBag.TotalExams = totalExams;
            ViewBag.TotalStarted = totalStarted;
            ViewBag.TotalCompleted = totalCompleted;
            return View();
        }

        public async Task<IActionResult> UniversityRanking()
        {
            return View();
        }

        public async Task<IActionResult> EducatorPerformance()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var educators = new List<ApplicationUser>();
            foreach (var user in allUsers) if (await _userManager.IsInRoleAsync(user, "Educator")) educators.Add(user);
            ViewBag.TotalEducators = educators.Count;
            ViewBag.TotalUniversities = educators.Select(e => e.University).Where(u => !string.IsNullOrEmpty(u)).Distinct().Count();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetExamCompletionStats()
        {
            var exams = await _db.Exams.ToListAsync();
            var stats = new List<object>();
            foreach (var exam in exams)
            {
                var attempts = _db.StudentExamAttempts.Where(a => a.ExamId == exam.Id);
                var started = await attempts.CountAsync();
                var completed = await attempts.CountAsync(a => a.SubmittedTimeUtc != null);
                stats.Add(new { examTitle = exam.Title, started, completed });
            }
            return Json(stats);
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversityRankingStats()
        {
            var attempts = await _db.StudentExamAttempts.Include(a => a.Student).Include(a => a.Exam).ThenInclude(e => e.Questions).Where(a => a.SubmittedTimeUtc != null).ToListAsync();
            var bestAttempts = attempts.GroupBy(a => new { a.StudentId, a.ExamId }).Select(g => g.OrderByDescending(a => a.Score).First()).ToList();
            var data = bestAttempts.GroupBy(a => a.Student!.University).Select(u => new { University = u.Key ?? "Unknown", Passed = u.Count(a => (decimal)a.Score / (decimal)a.Exam!.Questions.Sum(q => q.Points) * 100 >= 50), Failed = u.Count(a => (decimal)a.Score / (decimal)a.Exam!.Questions.Sum(q => q.Points) * 100 < 50) }).OrderByDescending(u => u.Passed).ToList();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetEducatorPerformanceStats()
        {
            var attempts = await _db.StudentExamAttempts.Include(a => a.Student).Include(a => a.Exam).ThenInclude(e => e.Questions).Include(a => a.Exam.Creator).ToListAsync();
            var bestAttempts = attempts.GroupBy(a => new { a.StudentId, a.ExamId }).Select(g => g.OrderByDescending(a => a.Score).First()).ToList();
            var allEducators = bestAttempts.GroupBy(a => a.Exam.CreatorId).Select(g =>
            {
                var educatorId = g.Key;
                var educatorName = g.First().Exam.Creator?.DisplayName ?? "Unknown";
                var statsByUniversity = g.GroupBy(a => a.Student.University ?? "Unknown").Select(u => new { University = u.Key, Passed = u.Count(a => (decimal)a.Score / (decimal)a.Exam.Questions.Sum(q => q.Points) * 100 >= 50), Failed = u.Count(a => (decimal)a.Score / (decimal)a.Exam.Questions.Sum(q => q.Points) * 100 < 50) }).ToList();
                return new { EducatorId = educatorId, EducatorName = educatorName, UniversityStats = statsByUniversity };
            }).ToList();
            return Json(allEducators);
        }
    }
}
