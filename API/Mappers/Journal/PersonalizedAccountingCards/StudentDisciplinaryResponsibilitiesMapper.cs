using Contracts.Journal.PersonalizedAccountingCards.StudentDisciplinaryResponsibilities;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class StudentDisciplinaryResponsibilitiesMapper
    {
        public static StudentDisciplinaryResponsibilityResponse ToResponse(this StudentDisciplinaryResponsibility record)
        {
            return new StudentDisciplinaryResponsibilityResponse(
                record.Id,
                record.Date,
                record.Misdemeanor,
                record.DisciplinaryResponsibilityKind,
                record.PersonalizedAccountingCardId
            );
        }

        public static StudentDisciplinaryResponsibility ToEntity(this UpdateStudentStudentDisciplinaryResponsibilityRequest request)
        {
            return new StudentDisciplinaryResponsibility
            {
                Date = request.Date,
                Misdemeanor = request.Misdemeanor,
                DisciplinaryResponsibilityKind = request.DisciplinaryResponsibilityKind
            };
        }

        public static StudentDisciplinaryResponsibility ToEntity(this CreateStudentDisciplinaryResponsibilityRequest request)
        {
            return new StudentDisciplinaryResponsibility
            {
                Date = request.Date,
                Misdemeanor = request.Misdemeanor,
                DisciplinaryResponsibilityKind = request.DisciplinaryResponsibilityKind,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
