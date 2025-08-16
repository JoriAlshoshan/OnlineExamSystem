using OnlineExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.ViewModels
{
    public class ExamViewModel
    {
        private Exam o;

        public int Id { get; set; }
        [Required]
        [Display(Name = "Exam Name")]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public int Time { get; set; }

        public int GroupId { get; set; }

        public List<ExamViewModel> ExamList { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<Group> GroupList { get; set; }
        public ExamViewModel(ExamViewModel model)
        {
            Id = model.Id;
            Title = model.Title ?? "";
            Description = model.Description ?? "";
            StartDate = model.StartDate;
            Time = model.Time;
            GroupId = model.GroupId;
        }

        public ExamViewModel()
        {
        }

        public ExamViewModel(Exam o)
        {
            this.o = o;
        }

        public Exam ConvertViewModel(ExamViewModel vm)
        {
            return new Exam
            {
                Id = vm.Id,
                Title = vm.Title ?? "",
                Description = vm.Description ?? "",
                StartDate = vm.StartDate,
                Time = vm.Time,
                GroupId = vm.GroupId
            };
        }
    }
}
