namespace OnlineExamSystem.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Contact { get; set; }
    public string CVFileName { get; set; }
    public string PictureFileName { get; set; }
    public int? GroupsId { get; set; }
    public Group Group { get; set; } 

    public ICollection<ExamResult> ExamResult { get;set; } = new HashSet<ExamResult>();   

}
