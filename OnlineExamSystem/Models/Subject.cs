using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        // ✅ وصف اختياري للموضوع
        public string Description { get; set; }
    }
}
