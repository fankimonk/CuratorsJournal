using Contracts.Journal.PersonalizedAccountingCards.IndividualInformation;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class IndividualInformationRecordsMapper
    {
        public static IndividualInformationRecordResponse ToResponse(this IndividualInformationRecord record)
        {
            return new IndividualInformationRecordResponse(
                record.Id,
                record.ActivityName,
                record.StartDate,
                record.EndDate,
                record.Result,
                record.Note,
                record.ParticipationKind,
                record.PersonalizedAccountingCardId
            );
        }

        public static IndividualInformationRecord ToEntity(this UpdateIndividualInformationRecordRequest request)
        {
            return new IndividualInformationRecord
            {
                ActivityName = request.ActivityName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Result = request.Result,
                Note = request.Note,
                ParticipationKind = request.ParticipationKind
            };
        }

        public static IndividualInformationRecord ToEntity(this CreateIndividualInformationRecordRequest request)
        {
            return new IndividualInformationRecord
            {
                ActivityName = request.ActivityName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Result = request.Result,
                Note = request.Note,
                ParticipationKind = request.ParticipationKind,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
