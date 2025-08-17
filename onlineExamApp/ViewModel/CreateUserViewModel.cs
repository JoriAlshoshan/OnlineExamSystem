using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.ViewModel
{
    public class CreateUserViewModel
    {
        [Required]
        public string DisplayName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? University { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public List<string> Roles { get; set; } = new();
        public List<string> AllRoles { get; set; } = new() { "Admin", "Educator", "Student" };
    }
}
