namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public class ParticipationInPedagogicalSeminarsRecordResponse(
        int id, DateOnly date, string topic,
        string participationForm, string seminarLocation,
        string? note)
    {
        public int Id { get; set; } = id;

        public DateOnly Date { get; set; } = date;

        public string Topic { get; set; } = topic;
        public string ParticipationForm { get; set; } = participationForm;
        public string SeminarLocation { get; set; } = seminarLocation;
        public string? Note { get; set; } = note;
    }
}
