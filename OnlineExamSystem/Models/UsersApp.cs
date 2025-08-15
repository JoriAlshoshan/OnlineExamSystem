using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSystem.Models;

public class UsersApp : IdentityUser
{
    public string FullName { get; set; }
    public string? ProfileImagePath { get; set; }

    public string? Role { get; set; }

    public string? University { get; set; }

}
