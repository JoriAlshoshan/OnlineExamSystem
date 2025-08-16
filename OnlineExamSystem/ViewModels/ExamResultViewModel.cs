namespace OnlineExamSystem.ViewModels
{
    public class ExamResultViewModel
    {
        public int StudentId { get; set; }

        public string ExamName { get; set; }

        public int TotalQuetion { get; set; }

        public int CorrectAnswer { get; set; }

        public int WrongAnswer { get; set; }
    }
}
