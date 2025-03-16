using Contracts.PEGroups;
using Domain.Entities;

namespace API.Mappers
{
    public static class PEGroupsMapper
    {
        public static PEGroupResponse ToResponse(this PEGroup position)
        {
            return new PEGroupResponse(
                position.Id, position.Name
            );
        }

        public static PEGroup ToEntity(this CreatePEGroupRequest request)
        {
            return new PEGroup
            {
                Name = request.Name
            };
        }

        public static PEGroup ToEntity(this UpdatePEGroupRequest request)
        {
            return new PEGroup
            {
                Name = request.Name
            };
        }
    }
}
