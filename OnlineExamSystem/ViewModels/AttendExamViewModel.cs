namespace OnlineExamSystem.ViewModels
{
    public class AttendExamViewModel
    {
        public int StudentId { get; set; }
        public string ExamName { get; set; }
        public List<QnAViewModel> QnA { get; set; }
        public string Message { get; set; }
    }
}
