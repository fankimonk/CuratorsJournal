using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGroupsService
    {
        Task<Group?> CreateGroup(string number, int specialtyId, int admissionYear);
        Task<Group?> AppointCurator(int groupId, int curatorId);
    }
}
