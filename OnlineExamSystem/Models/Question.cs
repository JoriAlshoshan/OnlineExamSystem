using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models;

public class Question
{
    [Key]
    public int QuestionId { get; set; }

    [ForeignKey("Exam")]
    public int ExamId { get; set; }

    [Required]
    public string QuestionText { get; set; }

    [Required]
    [MaxLength(50)]
    public string QuestionType { get; set; } 

    public virtual Exam Exam { get; set; }
}
