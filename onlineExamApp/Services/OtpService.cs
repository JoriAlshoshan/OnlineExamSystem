using System;

namespace onlineExamApp.Services
{
    public class OtpService
    {
        public (string Code, DateTime Expiry) GenerateOtp()
        {
            var otp = new Random().Next(1000, 9999).ToString(); 
            var expiry = DateTime.UtcNow.AddMinutes(5);
            return (otp, expiry);
        }
    }
}
