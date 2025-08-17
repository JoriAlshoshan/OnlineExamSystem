namespace onlineExamApp.ViewModel
{
    public class SubmitDto
    {
        public int AttemptId { get; set; }
        public List<QuestionAnswerDto> Answers { get; set; } = new();
    }
    public class QuestionAnswerDto { public int QuestionId { get; set; } public int? SelectedOptionId { get; set; } }

}
