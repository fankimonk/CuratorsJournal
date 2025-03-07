using Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class StudentDisciplinaryResponsibilitiesMapper
    {
        public static UpdateStudentStudentDisciplinaryResponsibilityRequest ToRequest(this StudentDisciplinaryResponsibilityResponse record)
        {
            return new UpdateStudentStudentDisciplinaryResponsibilityRequest(
                record.Date,
                record.Misdemeanor,
                record.DisciplinaryResponsibilityKind
            );
        }
    }
}
