using Contracts.Groups;
using Domain.Entities;

namespace API.Mappers
{
    public static class GroupsMapper
    {
        public static GroupResponse ToResponse(this Group group)
        {
            return new GroupResponse(
                group.Id, group.Number, group.AdmissionYear, group.SpecialtyId, group.CuratorId
            );
        }

        public static Group ToEntity(this CreateGroupRequest request)
        {
            return new Group
            {
                Number = request.Number,
                AdmissionYear = request.AdmissionYear,
                SpecialtyId = request.SpecialtyId,
                CuratorId = request.CuratorId
            };
        }

        public static Group ToEntity(this UpdateGroupRequest request)
        {
            return new Group
            {
                Number = request.Number,
                AdmissionYear = request.AdmissionYear,
                SpecialtyId = request.SpecialtyId,
                CuratorId = request.CuratorId
            };
        }
    }
}
