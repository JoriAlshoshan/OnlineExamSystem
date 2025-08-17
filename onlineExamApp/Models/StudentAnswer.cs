namespace onlineExamApp.Models
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public int AttemptId { get; set; }
        public StudentExamAttempt? Attempt { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public int? SelectedOptionId { get; set; }
        public Option? SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
    }
}
