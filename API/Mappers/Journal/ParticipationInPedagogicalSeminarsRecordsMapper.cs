using Contracts.Journal.CuratorsParticipationInPedagogicalSeminars;
using Domain.Entities.JournalContent;

namespace API.Mappers.Journal
{
    public static class ParticipationInPedagogicalSeminarsRecordsMapper
    {
        public static ParticipationInPedagogicalSeminarsRecordResponse ToResponse(
            this CuratorsParticipationInPedagogicalSeminarsRecord record)
        {
            return new ParticipationInPedagogicalSeminarsRecordResponse(
                record.Id, record.Date, record.Topic, record.ParticipationForm, record.SeminarLocation, record.Note
            );
        }

        public static CuratorsParticipationInPedagogicalSeminarsRecord ToEntity(this UpdateParticipationInPedagogicalSeminarsRecordRequest request)
        {
            return new CuratorsParticipationInPedagogicalSeminarsRecord
            {
                Date = request.Date,
                Topic = request.Topic,
                ParticipationForm = request.ParticipationForm,
                SeminarLocation = request.SeminarLocation,
                Note = request.Note
            };
        }

        public static CuratorsParticipationInPedagogicalSeminarsRecord ToEntity(this CreateParticipationInPedagogicalSeminarsRecordRequest request)
        {
            return new CuratorsParticipationInPedagogicalSeminarsRecord
            {
                Date = request.Date,
                Topic = request.Topic,
                ParticipationForm = request.ParticipationForm,
                SeminarLocation = request.SeminarLocation,
                Note = request.Note,
                PageId = request.PageId
            };
        }
    }
}
