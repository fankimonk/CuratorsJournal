using Contracts.Group;
using Domain.Entities;

namespace API.Mappers
{
    public static class GroupsMapper
    {
        public static GroupResponse ToResponse(this Group group)
        {
            return new GroupResponse(
                group.Id, group.Number, group.AdmissionYear, group.SpecialtyId
            );
        }

        public static Group ToEntity(this CreateGroupRequest request)
        {
            return new Group
            {
                Number = request.Number,
                AdmissionYear = request.AdmissionYear,
                SpecialtyId = request.SpecialtyId
            };
        }
    }
}
