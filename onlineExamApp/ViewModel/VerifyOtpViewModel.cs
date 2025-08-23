using System.ComponentModel.DataAnnotations;
using Microsoft.ApplicationInsights.AspNetCore;

namespace onlineExamApp.ViewModel
{
    public class VerifyOtpViewModel
    {
        public string Email { get; set; }

        public string Code { get; set; }
    }
}
