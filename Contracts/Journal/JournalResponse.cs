using Contracts.Curator;

namespace Contracts.Journal
{
    public class JournalResponse(int id, string groupNumber, CuratorResponse? curator)
    {
        public int Id { get; set; } = id;
        public string GroupNumber { get; set; } = groupNumber;
        public CuratorResponse? Curator { get; set; } = curator;
    }
}
