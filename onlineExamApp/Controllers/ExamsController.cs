using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineExamApp.Data;
using onlineExamApp.Models;
using onlineExamApp.ViewModel;
using SendGrid.Helpers.Mail;

namespace onlineExamApp.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult EducatorPage() => View();

        [Authorize(Roles = "Educator,Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Educator,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = _userManager.GetUserId(User)!;

            var exam = new Exam
            {
                Title = vm.Title,
                Description = vm.Description,
                CreatorId = userId,
                DurationMinutes = vm.DurationMinutes,
                StartTimeUtc = vm.StartTimeUtc,
                EndTimeUtc = vm.EndTimeUtc,
                Subject = vm.Subject,
                Difficulty = vm.Difficulty,
                MaxAttempts = vm.MaxAttempts,
                IsPublished = true
            };

            _db.Exams.Add(exam);
            await _db.SaveChangesAsync();

            return RedirectToAction("ManageQuestions", new { examId = exam.Id });
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var exam = await _db.Exams
                .Include(e => e.Questions).ThenInclude(q => q.Options)
                .Include(e => e.Creator)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null) return NotFound();

            var studentAttempts = await _db.StudentExamAttempts
                .Include(a => a.Student)
                .Where(a => a.ExamId == id && a.SubmittedTimeUtc != null)
                .ToListAsync();

            var vm = new ExamDetailsViewModel
            {
                Exam = exam,
                StudentAttempts = studentAttempts
            };

            return View(vm);
        }

        [Authorize(Roles = "Educator,Admin")]
        [Route("Exams/ManageQuestions/{examId}")]
        public async Task<IActionResult> ManageQuestions(int examId)
        {
            var exam = await _db.Exams
                .Include(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == examId);

            if (exam == null) return NotFound();

            ViewBag.ExamId = examId;
            return View(exam);
        }

        [HttpPost]
        [Authorize(Roles = "Educator,Admin")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto dto)
        {
            var exam = await _db.Exams.FindAsync(dto.ExamId);
            if (exam == null) return NotFound();

            var q = new Question
            {
                ExamId = dto.ExamId,
                Text = dto.Text,
                Points = dto.Points,
                Type = dto.Type == "TrueFalse" ? QuestionType.TrueFalse : QuestionType.MCQ
            };

            _db.Questions.Add(q);
            await _db.SaveChangesAsync();

            foreach (var o in dto.Options)
            {
                var opt = new Option { QuestionId = q.Id, Text = o.Text, IsCorrect = o.IsCorrect };
                _db.Options.Add(opt);
            }

            await _db.SaveChangesAsync();
            return Ok(new { questionId = q.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StartAttempt([FromForm] int examId)
        {
            var exam = await _db.Exams.FindAsync(examId);
            if (exam == null) return NotFound();

            var now = DateTime.UtcNow;

            if (now < exam.StartTimeUtc || now > exam.EndTimeUtc)
                return BadRequest("Exam is not currently available.");

            var userId = _userManager.GetUserId(User)!;

            var attemptsCount = await _db.StudentExamAttempts
                .CountAsync(a => a.StudentId == userId && a.ExamId == examId);

            if (attemptsCount >= exam.MaxAttempts)
                return BadRequest($"You have reached the maximum allowed attempts ({exam.MaxAttempts}).");

            var attempt = new StudentExamAttempt
            {
                ExamId = examId,
                StudentId = userId,
                StartTimeUtc = now,
                EndTimeUtc = now.AddMinutes(exam.DurationMinutes)
            };

            _db.StudentExamAttempts.Add(attempt);
            await _db.SaveChangesAsync();

            return Json(new
            {
                attemptId = attempt.Id,
                endTimeUtc = attempt.EndTimeUtc.ToString("o"),
                remainingAttempts = exam.MaxAttempts - attemptsCount - 1
            });
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SavePartialAnswers([FromBody] SubmitDto dto)
        {
            var attempt = await _db.StudentExamAttempts.Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == dto.AttemptId);
            if (attempt == null) return NotFound();

            _db.StudentAnswers.RemoveRange(attempt.Answers);

            foreach (var a in dto.Answers)
            {
                var ans = new StudentAnswer { AttemptId = attempt.Id, QuestionId = a.QuestionId, SelectedOptionId = a.SelectedOptionId };
                _db.StudentAnswers.Add(ans);
            }
            await _db.SaveChangesAsync();
            return Ok(new { saved = true, serverTimeUtc = DateTime.UtcNow.ToString("o") });
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitAttempt([FromBody] SubmitDto dto)
        {
            var attempt = await _db.StudentExamAttempts
                .Include(a => a.Exam).ThenInclude(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(a => a.Id == dto.AttemptId);

            if (attempt == null) return NotFound();
            if (attempt.IsSubmitted) return BadRequest("This attempt was already submitted.");

            var userId = _userManager.GetUserId(User)!;
            if (attempt.StudentId != userId) return Unauthorized();

            decimal score = 0;
            var answerResults = new List<object>();

            foreach (var ans in dto.Answers)
            {
                var question = attempt.Exam.Questions.FirstOrDefault(q => q.Id == ans.QuestionId);
                if (question == null) continue;

                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                bool isCorrect = (ans.SelectedOptionId == correctOption?.Id);

                if (isCorrect) score += question.Points;

                answerResults.Add(new
                {
                    questionId = ans.QuestionId,
                    selectedOptionId = ans.SelectedOptionId,
                    isCorrect = isCorrect
                });
            }

            attempt.Score = score;
            attempt.SubmittedTimeUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var attemptsCount = await _db.StudentExamAttempts
                .CountAsync(a => a.StudentId == userId && a.ExamId == attempt.ExamId && a.SubmittedTimeUtc != null);

            int remainingAttempts = attempt.Exam.MaxAttempts - attemptsCount;

            return Json(new
            {
                score = score,
                totalScore = attempt.Exam.Questions.Sum(q => q.Points),
                answers = answerResults,
                remainingAttempts = remainingAttempts
            });
        }

        public IActionResult EditQuestion(int id)
        {
            var question = _db.Questions.Include(q => q.Options).FirstOrDefault(q => q.Id == id);
            if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditQuestion(Question model)
        {
            var q = _db.Questions.Include(x => x.Options).FirstOrDefault(x => x.Id == model.Id);
            if (q == null) return NotFound();

            q.Text = model.Text;
            q.Points = model.Points;

            var optionIdsFromModel = model.Options.Select(o => o.Id).ToList();
            var optionsToRemove = q.Options.Where(o => !optionIdsFromModel.Contains(o.Id)).ToList();
            _db.Options.RemoveRange(optionsToRemove);

            foreach (var optionModel in model.Options)
            {
                if (optionModel.Id == 0)
                {
                    q.Options.Add(new Option { QuestionId = q.Id, Text = optionModel.Text, IsCorrect = optionModel.IsCorrect });
                }
                else
                {
                    var existingOption = q.Options.FirstOrDefault(o => o.Id == optionModel.Id);
                    if (existingOption != null)
                    {
                        existingOption.Text = optionModel.Text;
                        existingOption.IsCorrect = optionModel.IsCorrect;
                    }
                }
            }

            _db.SaveChanges();
            return RedirectToAction("ManageQuestions", new { examId = q.ExamId });
        }

        public IActionResult DeleteQuestion(int id)
        {
            var question = _db.Questions.Include(q => q.Options).FirstOrDefault(q => q.Id == id);
            if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteQuestionConfirmed(int id)
        {
            var question = _db.Questions.Include(q => q.Options).FirstOrDefault(q => q.Id == id);
            if (question == null) return NotFound();

            _db.Options.RemoveRange(question.Options);
            _db.Questions.Remove(question);
            _db.SaveChanges();

            return RedirectToAction("ManageQuestions", new { examId = question.ExamId });
        }

        [Authorize(Roles = "Educator")]
        public async Task<IActionResult> MyExams()
        {
            var userId = _userManager.GetUserId(User);
            var exams = await _db.Exams.Where(e => e.CreatorId == userId).ToListAsync();
            return View(exams);
        }

        [HttpPost]
        [Authorize(Roles = "Educator,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _db.Exams.Include(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null) return NotFound();

            try
            {
                foreach (var question in exam.Questions)
                {
                    _db.Options.RemoveRange(question.Options);
                }

                _db.Questions.RemoveRange(exam.Questions);
                _db.Exams.Remove(exam);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(MyExams));
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Unable to delete this exam as it is associated with student attempts.";
                return RedirectToAction(nameof(MyExams));
            }
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Take(int id)
        {
            var exam = await _db.Exams.Include(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return NotFound();
            return View("Take", exam);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> TakeAr(int id)
        {
            var exam = await _db.Exams.Include(e => e.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (exam == null) return NotFound();
            return View("TakeAr", exam);
        }
    }
}
