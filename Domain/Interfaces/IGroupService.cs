using Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.IServices
{
    public interface IGroupService
    {
        Task<GroupDTO> AddGroupAsync(CreateGroupDTO dto);
        Task<GroupDTO> GetGroupByIdAsync(string id);
        Task<IReadOnlyList<GroupDTO>> GetAllGroupsAsync();
        Task<GroupDTO> UpdateGroupAsync(string id, UpdateGroupDTO dto);
        Task DeleteGroupAsync(string id);
        Task<IReadOnlyList<GroupDTO>> GetGroupsByCourseIdAsync(string courseId);
        Task<IReadOnlyList<GroupDTO>> GetGroupsByInstructorIdAsync(string instructorId);
    }
}