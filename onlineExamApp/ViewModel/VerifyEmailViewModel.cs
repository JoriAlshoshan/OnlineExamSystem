using System.ComponentModel.DataAnnotations;

namespace onlineExamApp.ViewModel
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
