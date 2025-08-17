namespace onlineExamApp.ViewModel
{
    public class QuestionDto
    {
        public int ExamId { get; set; }
        public string Text { get; set; } = "";
        public string Type { get; set; } = "MCQ";
        public decimal Points { get; set; } = 1m;
        public List<OptionDto> Options { get; set; } = new();
    }

    public class OptionDto { public string Text { get; set; } = ""; public bool IsCorrect { get; set; } = false; }

}
