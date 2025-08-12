using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace OnlineExamSystem.Models
{
    public class UsersApp : IdentityUser
    {
        public string fullName { get; set; }

    }
}
