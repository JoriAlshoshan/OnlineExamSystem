using System.Collections.Generic;

namespace OnlineExamSystem.ViewModels
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }

        public string ExamTitle { get; set; }

        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}