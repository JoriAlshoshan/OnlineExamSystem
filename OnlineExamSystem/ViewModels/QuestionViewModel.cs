using System.Collections.Generic;

namespace OnlineExamSystem.ViewModels
{

    public class QuestionViewModel
    {
  
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }


        public string QuestionType { get; set; }

  
        public List<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();

        public int? SelectedOptionId { get; set; }
    }
}