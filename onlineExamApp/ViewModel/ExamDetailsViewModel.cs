using onlineExamApp.Models;
using System.Collections.Generic;

namespace onlineExamApp.ViewModel
{
    public class ExamDetailsViewModel
    {
        public Exam Exam { get; set; } = null!;
        public List<StudentExamAttempt> StudentAttempts { get; set; } = new List<StudentExamAttempt>();
    }
}

