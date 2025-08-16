namespace OnlineExamSystem.Models;

public class QnA
{
    public int Id { get; set; } 
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    public string Question { get; set; }
    public int Answer {  get; set; }
    public string option1 { get; set; }
    public string option2 { get; set; }
    public string option3 { get; set; }
    public string option4 { get; set; }
    public ICollection<ExamResult> ExamResult { get; set; } = new HashSet<ExamResult>();  
     
}
