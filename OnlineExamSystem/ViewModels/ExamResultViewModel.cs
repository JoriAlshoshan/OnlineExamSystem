using System.Collections.Generic;

namespace OnlineExamSystem.ViewModels
{

    public class ExamResultViewModel
    {
        public int ExamId { get; set; }

        public string ExamTitle { get; set; }

        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public double Score { get; set; }

        public List<QuestionResultViewModel> QuestionResults { get; set; } = new List<QuestionResultViewModel>();
    }
}