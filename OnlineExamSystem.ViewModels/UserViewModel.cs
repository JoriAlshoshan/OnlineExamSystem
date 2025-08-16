using System;
using System.ComponentModel.DataAnnotations;
using OnlineExamSystem.Models;

namespace OnlineExamSystem.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(UsersApp model)
        {
            Id = model.Id;
            Name = model.Name ?? "";
            UserName = model.UserName;
            Password = model.Password;
            Role = model.Role;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
