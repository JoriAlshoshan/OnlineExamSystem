using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? University { get; set; }

        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
    }
}

