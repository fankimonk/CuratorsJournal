namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public class IdeologicalEducationalWorkPageResponse(int pageId, 
        IdeologicalAndEducationalWorkAttributesResponse attributes,
        List<IdeologicalEducationalWorkRecordResponse> ideologicalEducationalWork)
    {
        public int PageId { get; set; } = pageId;

        public IdeologicalAndEducationalWorkAttributesResponse Attributes { get; set; } = attributes;
 
        public List<IdeologicalEducationalWorkRecordResponse> IdeologicalEducationalWork { get; set; } = ideologicalEducationalWork;
    }
}
