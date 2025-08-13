using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Data;
using OnlineExamSystem.Models;
using OnlineExamSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamSystem.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var now = DateTime.UtcNow;
            var exams = await _context.Exams
                .Where(e => e.StartTime <= now && e.EndTime >= now)
                .Include(e => e.Subject)
                .ToListAsync();
            return View(exams);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Take(int id)
        {
            var exam = await _context.Exams
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(e => e.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            var now = DateTime.UtcNow;
            if (now < exam.StartTime || now > exam.EndTime)
            {
                return View("ExamNotAvailable", exam);
            }

            var questions = await _context.Questions
                .Where(q => q.ExamId == id)
                .ToListAsync();

            var questionIds = questions.Select(q => q.QuestionId).ToList();
            var options = await _context.Options
                .Where(o => questionIds.Contains(o.QuestionId))
                .ToListAsync();

            var viewModel = new ExamViewModel
            {
                ExamId = exam.ExamId,
                ExamTitle = exam.Title,
                Questions = questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = options.Where(o => o.QuestionId == q.QuestionId)
                        .Select(o => new OptionViewModel
                        {
                            OptionId = o.OptionId,
                            OptionText = o.OptionText
                        }).ToList(),
                    SelectedOptionId = null
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Take(ExamViewModel model)
        {
            if (model == null || model.Questions == null)
            {
                return BadRequest();
            }

            int totalQuestions = model.Questions.Count;
            int correctCount = 0;
            var questionResults = new List<QuestionResultViewModel>(totalQuestions);

            foreach (var question in model.Questions)
            {
                Option? selectedOption = null;
                if (question.SelectedOptionId.HasValue)
                {
                    selectedOption = await _context.Options
                        .AsNoTracking()
                        .FirstOrDefaultAsync(o => o.OptionId == question.SelectedOptionId.Value);
                }

                var correctOption = await _context.Options
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.QuestionId == question.QuestionId && o.IsCorrect);

                bool isCorrect = selectedOption != null && selectedOption.OptionId == correctOption?.OptionId;
                if (isCorrect)
                {
                    correctCount++;
                }

                questionResults.Add(new QuestionResultViewModel
                {
                    QuestionText = question.QuestionText,
                    SelectedOptionText = selectedOption?.OptionText,
                    CorrectOptionText = correctOption?.OptionText,
                    IsCorrect = isCorrect
                });
            }

            double score = totalQuestions > 0
                ? (double)correctCount / totalQuestions * 100.0
                : 0.0;

            var resultViewModel = new ExamResultViewModel
            {
                ExamId = model.ExamId,
                ExamTitle = model.ExamTitle,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctCount,
                Score = score,
                QuestionResults = questionResults
            };

            return View("Result", resultViewModel);
        }
    }
}
