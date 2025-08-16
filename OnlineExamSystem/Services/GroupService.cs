using Microsoft.IdentityModel.Abstractions;
using OnlineExamSystem.UnitOfWork;
using OnlineExamSystem.ViewModels;
using System.Linq;
using System.Text.RegularExpressions;

namespace OnlineExamSystem.Services
{
    public class GroupService : IGroupService
    {
        IUnitOfWork _unitOfWork;
        ILogger<GroupService> _iLogger;

        public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> Ilogger)
        {
            _unitOfWork = unitOfWork;
            _iLogger = Ilogger;
        }

        public async Task<GroupViewModel> AddGroupAsync(GroupViewModel groupVM)
        {
            try
            {
                GroupViewModel objGroup = groupVM.ConvertGroupsViewModel(groupVM);
                await _unitOfWork.GenericRepository<Group>().AddAsync(objGroup);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return null;
            }
            return groupVM;
        }

        public PagedResult<GroupViewModel> GetAllGroups(int pageNumber, int pageSize)
        {
            var model = new GroupViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<GroupViewModel> deatailList = new List<GroupViewModel>();
                var modelList = _unitOfWork.GenericRepository<Group>().GetAll().Skip(ExcludeRecords).
                    Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Group>().GetAll().ToList();
                deatailList = GroupListInfo(modelList);
                if (deatailList != null)
                {
                    model.GroupList = deatailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);
            }
            var result = new PagedResult<GroupViewModel>
            {
                Data = model.GroupList,
                TotalItem = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;
            
        }

        private List<GroupViewModel> GroupListInfo(List<Group> modelList)
        {
            return modelList.Select(o => new GroupViewModel(o)).ToList();
        }

        public IEnumerable<Group> GetAllGroups()
        {
            try
            {
                var groups=_unitOfWork.GenericRepository<Group>().GetAll();
                return groups;
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);

            }
            return Enumerable.Empty<Group>();
        }

        public GroupViewModel GetById(int groupId)
        {
            try
            {
                var group = _unitOfWork.GenericRepository<Group>().GetByID(groupId);   
                return new GroupViewModel(group);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(ex.Message);  
               
            }
            return null;
        }
    }
}
