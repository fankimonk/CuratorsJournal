using Contracts.Journal.PersonalizedAccountingCards.IndividualWorkWithStudents;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class IndividualWorkWithStudentMapper
    {
        public static IndividualWorkWithStudentRecordResponse ToResponse(this IndividualWorkWithStudentRecord record)
        {
            return new IndividualWorkWithStudentRecordResponse(
                record.Id,
                record.Date,
                record.WorkDoneAndRecommendations,
                record.Result,
                record.PersonalizedAccountingCardId
            );
        }

        public static IndividualWorkWithStudentRecord ToEntity(this UpdateIndividualWorkWithStudentRecordRequest request)
        {
            return new IndividualWorkWithStudentRecord
            {
                Date = request.Date,
                WorkDoneAndRecommendations = request.WorkDoneAndRecommendations,
                Result = request.Result
            };
        }

        public static IndividualWorkWithStudentRecord ToEntity(this CreateIndividualWorkWithStudentRecordRequest request)
        {
            return new IndividualWorkWithStudentRecord
            {
                Date = request.Date,
                WorkDoneAndRecommendations = request.WorkDoneAndRecommendations,
                Result = request.Result,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
