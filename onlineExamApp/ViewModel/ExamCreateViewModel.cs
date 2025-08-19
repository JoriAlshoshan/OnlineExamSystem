using onlineExamApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.ViewModel
{
    public class ExamCreateViewModel
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; } = "";
        public string? Description { get; set; }
        [Required] public DateTime StartTimeUtc { get; set; }
        [Required] public DateTime EndTimeUtc { get; set; }
        [Required] public int DurationMinutes { get; set; }
        public string? Subject { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        [Range(1, 10, ErrorMessage = "Attempts must be between 1 and 10")]
        public int MaxAttempts { get; set; } = 1;
    }
}

