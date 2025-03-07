using Contracts.Journal.PersonalizedAccountingCards.IndividualWorkWithStudents;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class IndividualWorkWithStudentMapper
    {
        public static UpdateIndividualWorkWithStudentRecordRequest ToRequest(this IndividualWorkWithStudentRecordResponse record)
        {
            return new UpdateIndividualWorkWithStudentRecordRequest(
                record.Date,
                record.WorkDoneAndRecommendations,
                record.Result
            );
        }
    }
}
