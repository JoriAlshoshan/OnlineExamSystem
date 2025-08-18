using onlineExamApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string CreatorId { get; set; } = null!;
        public ApplicationUser? Creator { get; set; }

        public int DurationMinutes { get; set; } = 30;
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }

        public string? Subject { get; set; }
        public DifficultyLevel? Difficulty { get; set; }

        public bool IsPublished { get; set; } = false;

        public int MaxAttempts { get; set; } = 1;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}

