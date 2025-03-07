using Contracts.Journal.PersonalizedAccountingCards.WorkWithParents;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class WorkWithParentsMapper
    {
        public static UpdateWorkWithParentsRecordRequest ToRequest(this WorkWithParentsRecordResponse record)
        {
            return new UpdateWorkWithParentsRecordRequest(
                record.Date,
                record.WorkContent,
                record.Note
            );
        }
    }
}
