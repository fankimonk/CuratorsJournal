using Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class StudentEcouragementsMapper
    {
        public static UpdateStudentEncouragementRequest ToRequest(this StudentEncouragementResponse record)
        {
            return new UpdateStudentEncouragementRequest(
                record.Date,
                record.Achievement,
                record.EncouragementKind
            );
        }
    }
}
