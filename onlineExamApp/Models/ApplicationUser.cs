using Microsoft.AspNetCore.Identity;

namespace onlineExamApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? University { get; set; }



    }
}
