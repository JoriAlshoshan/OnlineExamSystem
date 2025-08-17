using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }

        [Required] 
        public string Text { get; set; } = null!;
        public QuestionType Type { get; set; } = QuestionType.MCQ;
        public decimal Points { get; set; } = 1m;
        public List<Option> Options { get; set; } = new List<Option>();

        public ICollection<StudentAnswer> StudentAnswers { get; set; }

    }
}
