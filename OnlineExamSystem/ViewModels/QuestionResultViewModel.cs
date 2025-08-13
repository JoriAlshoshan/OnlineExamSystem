namespace OnlineExamSystem.ViewModels
{
    public class QuestionResultViewModel
    {
        public string QuestionText { get; set; }

        public string? SelectedOptionText { get; set; }

        public string? CorrectOptionText { get; set; }

        public bool IsCorrect { get; set; }
    }
}