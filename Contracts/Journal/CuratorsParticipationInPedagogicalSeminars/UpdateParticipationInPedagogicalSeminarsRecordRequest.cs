namespace Contracts.Journal.CuratorsParticipationInPedagogicalSeminars
{
    public record UpdateParticipationInPedagogicalSeminarsRecordRequest
    (
        DateOnly? Date,

        string? Topic,
        string? ParticipationForm,
        string? SeminarLocation,

        string? Note
    );
}
