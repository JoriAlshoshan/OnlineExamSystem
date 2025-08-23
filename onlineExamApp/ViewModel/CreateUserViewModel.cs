using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        public string DisplayName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string?University { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public List<string> AllRoles { get; set; } = new() { "Admin", "Educator", "Student" };
    }
}
