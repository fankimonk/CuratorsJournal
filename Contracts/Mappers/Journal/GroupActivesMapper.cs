using Contracts.Journal.GroupActives;

namespace Contracts.Mappers.Journal
{
    public static class GroupActivesMapper
    {
        public static UpdateGroupActiveRequest ToRequest(this GroupActiveResponse active)
        {
            return new UpdateGroupActiveRequest(
                active.PositionName,
                active.StudentId
            );
        }
    }
}
