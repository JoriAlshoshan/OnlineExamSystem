using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models;

public class Option
{
    [Key]
    public int OptionId { get; set; }

    [ForeignKey("Question")]
    public int QuestionId { get; set; }

    [Required]
    public string OptionText { get; set; }

    [Required]
    public bool IsCorrect { get; set; }

    public virtual Question Question { get; set; }
}
