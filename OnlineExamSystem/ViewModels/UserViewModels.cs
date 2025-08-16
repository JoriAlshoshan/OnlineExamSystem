using OnlineExamSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }
        public UserViewModel(UsersApp model)
        {
            //Id = model.Id;
            //UserName = model.UserName;
            //Password = model.Password;
            Role = model.Role;
        }

       

        public UsersApp ConvertViewModel(UserViewModel vm)
        {
            return new UsersApp
            {
                UserName = vm.UserName,
                Role = vm.Role
            };
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public int Role { get; set; }
        public List<UserViewModel> UserList { get; set; }
        public int TotalCount { get; set; }
    }
}
