using OnlineExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.ViewModels
{
    public class QnAViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Exam")]
        public int ExamId { get; set; }
        [Required]
        [Display(Name = "Question")]
        public string Question { get; set; }
        [Required]
        [Display(Name = "Answer")]
        public int Answer { get; set; }
        [Required]
        [Display(Name = "option 1")]
        public string option1 { get; set; }
        [Required]
        [Display(Name = "option 2")]
        public string option2 { get; set; }
        [Required]
        [Display(Name = "option 3")]
        public string option3 { get; set; }
        [Required]
        [Display(Name = "option 4")]
        public string option4 { get; set; }

        public List<QnAViewModel> qnAList { get; set; }
        public IEnumerable<Exam> EXamList { get; set; }
        public int TotalCount { get; set; }
        public int SelectedAnswar { get; set; }

        public QnAViewModel(QnA model)
        {
            Id = model.Id;
            ExamId = model.ExamId;
            Question = model.Question ?? "";
            option1 = model.option1 ?? "";
            option2 = model.option2 ?? "";
            option3 = model.option3 ?? "";
            option4 = model.option4 ?? "";
            Answer = model.Answer;
        }

        public QnAViewModel()
        {
        }

        public QnA ConvertViewModel(QnAViewModel vm)
        {
            return new QnA
            {
                Id = vm.Id,
                ExamId = vm.ExamId,
                Question = vm.Question ?? "",
                option1 = vm.option1 ?? "",
                option2 = vm.option2 ?? "",
                option3 = vm.option3 ?? "",
                option4 = vm.option4 ?? "",
                Answer = vm.Answer
            };
        }

    }
}
