using onlineExamApp.Models;

namespace onlineExamApp.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? University { get; set; } 

        public IList<string> Roles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();


    }
}
