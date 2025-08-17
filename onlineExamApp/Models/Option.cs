namespace onlineExamApp.Models
{
    public class Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;
    }
}
