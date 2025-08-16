using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamSystem.Models
{
    public class UsersApp : IdentityUser
    {
        public int Role { get; set; }

        public ICollection<Group> Groups { get; set; } = new HashSet<Group>();
        public string FullName { get; set; }  
    }
}
