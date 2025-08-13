using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.Data;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exam/EducatorPage
        public async Task<IActionResult> EducatorPage()
        {
            var exams = await _context.Exams
                .Include(e => e.Subject)
                .Include(e => e.CreatedByUser)
                .ToListAsync();
            return View(exams);
        }
        // GET: Exam/Create
        public IActionResult Create()
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(new Exam());
        }

        // POST: Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {
            ViewBag.Subjects = _context.Subjects.ToList(); // مهم جدًا

            if (!ModelState.IsValid)
            {
                return View(exam);
            }

            exam.CreatedBy = User.Identity.Name;
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Exam created successfully!";
            return RedirectToAction("EducatorPage");
        }

        // GET: Exam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return NotFound();

            ViewBag.Subjects = _context.Subjects.ToList();
            return View(exam);
        }

        // POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exam exam)
        {
            if (id != exam.ExamId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(exam);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Exam updated successfully!";
                return RedirectToAction("EducatorPage", "Exam");
            }

            ViewBag.Subjects = _context.Subjects.ToList();
            return View(exam);
        }

        // GET: Exam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(m => m.ExamId == id);

            if (exam == null) return NotFound();

            return View(exam);
        }

        // POST: Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Exam deleted successfully!";
            }

            return RedirectToAction("EducatorPage", "Exam");
        }
    }
}
