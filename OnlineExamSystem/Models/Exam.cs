using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamSystem.Models
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        [MaxLength(50)]
        public string Difficulty { get; set; }

        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 minute.")]
        public int DurationMinutes { get; set; } = 60;

        [Required]
        [ForeignKey("CreatedByUser")]
        public string CreatedBy { get; set; }

        [Required]
        public virtual UsersApp CreatedByUser { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        public bool IsPublished { get; set; } = false;
    }
}
