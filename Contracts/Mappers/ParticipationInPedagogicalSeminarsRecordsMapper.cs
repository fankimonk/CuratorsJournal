using Contracts.Journal.CuratorsParticipationInPedagogicalSeminars;

namespace Contracts.Mappers
{
    public static class ParticipationInPedagogicalSeminarsRecordsMapper
    {
        public static UpdateParticipationInPedagogicalSeminarsRecordRequest ToRequest(this ParticipationInPedagogicalSeminarsRecordResponse record)
        {
            return new UpdateParticipationInPedagogicalSeminarsRecordRequest(
                record.Date, record.Topic, record.ParticipationForm, record.SeminarLocation, record.Note
            );
        }
    }
}
