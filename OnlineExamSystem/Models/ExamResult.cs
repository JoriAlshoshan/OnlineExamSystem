namespace OnlineExamSystem.Models;

public class ExamResult
{
    public int Id { get; set; }
    public int StudentsId { get; set; }
    public Student Student { get; set; }
    public int? ExamId { get; set; }
    public Exam Exam { get; set; }
    public int QnAId {  get; set; }
    public QnA QnA { get; set; }  
    public int Answer {  get; set; }
    public string Title { get; internal set; }
}
