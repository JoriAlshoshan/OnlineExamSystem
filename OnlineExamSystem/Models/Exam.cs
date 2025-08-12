using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamSystem.Models;

public class Exam
{
    [Key]
    public int ExamId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }

    [ForeignKey("Subject")]
    public int SubjectId { get; set; }

    public virtual Subject Subject { get; set; }

    [MaxLength(50)]
    public string Difficulty { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public int DurationMinutes { get; set; }

    [ForeignKey("CreatedByUser")]
    public int CreatedBy { get; set; }

    // public virtual User CreatedByUser { get; set; }
}
