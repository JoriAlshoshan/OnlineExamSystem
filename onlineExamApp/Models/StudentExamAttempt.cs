namespace onlineExamApp.Models
{
    public class StudentExamAttempt
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = null!;
        public ApplicationUser? Student { get; set; }
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public DateTime? SubmittedTimeUtc { get; set; }
        public decimal Score { get; set; } = 0;
        public ICollection<StudentAnswer> Answers { get; set; } = new List<StudentAnswer>();
        public string? EducatorId { get; set; }
        public ApplicationUser? Educator { get; set; }

        public bool IsSubmitted => SubmittedTimeUtc != null;


    }
}
