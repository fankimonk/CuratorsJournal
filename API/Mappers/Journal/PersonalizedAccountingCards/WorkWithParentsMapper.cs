using Contracts.Journal.PersonalizedAccountingCards.WorkWithParents;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class WorkWithParentsMapper
    {
        public static WorkWithParentsRecordResponse ToResponse(this WorkWithParentsRecord record)
        {
            return new WorkWithParentsRecordResponse(
                record.Id,
                record.Date,
                record.WorkContent,
                record.Note,
                record.PersonalizedAccountingCardId
            );
        }

        public static WorkWithParentsRecord ToEntity(this UpdateWorkWithParentsRecordRequest request)
        {
            return new WorkWithParentsRecord
            {
                Date = request.Date,
                WorkContent = request.WorkContent,
                Note = request.Note
            };
        }

        public static WorkWithParentsRecord ToEntity(this CreateWorkWithParentsRecordRequest request)
        {
            return new WorkWithParentsRecord
            {
                Date = request.Date,
                WorkContent = request.WorkContent,
                Note = request.Note,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
