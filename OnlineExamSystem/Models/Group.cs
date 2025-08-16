namespace OnlineExamSystem.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserSId { get; set; }

    public UsersApp Users { get; set; }

    public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    public ICollection<Exam> Exam { get; set; } = new HashSet<Exam>();


}
