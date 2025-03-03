namespace Contracts.Journal.GroupActives
{
    public class GroupActivesPageResponse(int pageId, List<GroupActiveResponse> groupActives)
    {
        public int PageId { get; set; } = pageId;
        public List<GroupActiveResponse> GroupActives { get; set; } = groupActives;
    }
}
