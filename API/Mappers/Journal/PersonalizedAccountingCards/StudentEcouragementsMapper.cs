using Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class StudentEcouragementsMapper
    {
        public static StudentEncouragementResponse ToResponse(this StudentEcouragement record)
        {
            return new StudentEncouragementResponse(
                record.Id,
                record.Date,
                record.Achievement,
                record.EncouragementKind,
                record.PersonalizedAccountingCardId
            );
        }

        public static StudentEcouragement ToEntity(this UpdateStudentEncouragementRequest request)
        {
            return new StudentEcouragement
            {
                Date = request.Date,
                Achievement = request.Achievement,
                EncouragementKind = request.EncouragementKind
            };
        }

        public static StudentEcouragement ToEntity(this CreateStudentEncouragementRequest request)
        {
            return new StudentEcouragement
            {
                Date = request.Date,
                Achievement = request.Achievement,
                EncouragementKind = request.EncouragementKind,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
