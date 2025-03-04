namespace Contracts.Journal.Traditions
{
    public class TraditionsPageResponse(int pageId, List<TraditionResponse> traditions)
    {
        public int PageId { get; set; } = pageId;

        public List<TraditionResponse> Traditions { get; set; } = traditions;
    }
}
