using Contracts.Journal.GroupActives;
using Domain.Entities.JournalContent;

namespace API.Mappers
{
    public static class GroupActivesMapper
    {
        public static GroupActiveResponse ToResponse(this GroupActive active)
        {
            return new GroupActiveResponse(
                active.Id, active.PositionName, active.StudentId
            );
        }

        public static GroupActive ToEntity(this UpdateGroupActiveRequest request)
        {
            return new GroupActive
            {
                PositionName = request.PositionName,
                StudentId = request.StudentId
            };
        }

        public static GroupActive ToEntity(this CreateGroupActiveRequest request)
        {
            return new GroupActive
            {
                PositionName = request.PositionName,
                StudentId = request.StudentId,
                PageId = request.PageId
            };
        }
    }
}
