using OnlineExamSystem.ViewModels;
using System.Text.RegularExpressions;

namespace OnlineExamSystem.Services
{
    public interface IGroupService
    {
        PagedResult<GroupViewModel> GetAllGroups(int pageNumber, int pageSize);
        Task <GroupViewModel> AddGroupAsync(GroupViewModel groupVM);
        IEnumerable<Group> GetAllGroups();
        GroupViewModel GetById(int groupId);
    }


}
