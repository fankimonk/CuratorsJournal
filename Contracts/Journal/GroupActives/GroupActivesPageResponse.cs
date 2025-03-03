namespace Contracts.Journal.GroupActives
{
    public class GroupActivesPageResponse(int pageId, List<GroupActiveResponse> groupActives)
    {
        public int PageId { get; set; } = pageId;
        public List<GroupActiveResponse> GroupActives { get; set; } = groupActives;
    }

    public class GroupActiveResponse(int id, string positionName, int studentId)
    {
        public int Id { get; set; } = id;

        public string PositionName { get; set; } = positionName;

        public int StudentId { get; set; } = studentId;
    }
}
