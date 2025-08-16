using OnlineExamSystem.Models;
using OnlineExamSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.ViewModels
{
    public class GroupViewModel
    {
        private System.Text.RegularExpressions.Group o;

        public int Id { get; set; }
        [Required]
        [Display(Name = "Group Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int UserSId { get; set; }

        public List<GroupViewModel> GroupList { get; set; }

        public int TotalCount { get; set; }
        public List<StudentCheckBoxListViewModel> StudentCheckList { get; set; }
        public GroupViewModel(GroupViewModel model)
        {
            Id = model.Id;
            Name = model.Name ?? "";
            Description = model.Description ?? "";
            UserSId = model.UserSId;
        }

        
        public GroupViewModel()
        {
        }

        public GroupViewModel(System.Text.RegularExpressions.Group o)
        {
            this.o = o;
        }

        public Group ConvertViewModel(GroupViewModel vm)
        {
            return new Group
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                Description = vm.Description ?? "",
                UserSId = vm.UserSId.ToString()
            };
        }

        internal GroupViewModel ConvertGroupsViewModel(GroupViewModel groupVM)
        {
            throw new NotImplementedException();
        }
    }
}
