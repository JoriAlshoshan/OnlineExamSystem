using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamSystem.Models
{
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer
    }

    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        [ForeignKey("Exam")]
        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        public virtual ICollection<Option> Options { get; set; } = new List<Option>();
    }
}
