using Microsoft.AspNetCore.Identity;

namespace OnlineExamSystem.Models
{
    public class UsersApp : IdentityUser
    {
        public string FullName { get; set; }

        // ✅ دور المستخدم (يمكن استخدام IdentityRole أيضاً)
        public string Role { get; set; }
    }
}
