namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public class ParticipationInPedagogicalSeminarsPageResponse(int pageId, 
        List<ParticipationInPedagogicalSeminarsRecordResponse> participationInPedagogicalSeminars)
    {
        public int PageId { get; set; } = pageId;

        public List<ParticipationInPedagogicalSeminarsRecordResponse> ParticipationInPedagogicalSeminars { get; set; }
            = participationInPedagogicalSeminars;
    }
}
