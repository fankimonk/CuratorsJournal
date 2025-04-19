using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGroupsService
    {
        Task<Group?> CreateGroup(string number, int specialtyId, int admissionYear, int? curatorId);
        Task<Group?> AppointCurator(int groupId, int? curatorId);
    }
}
