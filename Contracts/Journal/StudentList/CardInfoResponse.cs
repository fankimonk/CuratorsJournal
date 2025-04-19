namespace Contracts.Journal.StudentList
{
    public class CardInfoResponse(int id, int pageId)
    {
        public int Id { get; set; } = id;
        public int PageId { get; set; } = pageId;
    }
}
