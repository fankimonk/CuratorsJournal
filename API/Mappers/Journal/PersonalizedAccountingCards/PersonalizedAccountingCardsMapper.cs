using Contracts.Journal.PersonalizedAccountingCards;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class PersonalizedAccountingCardsMapper
    {
        public static PersonalizedAccountingCardResponse ToResponse(this PersonalizedAccountingCard card)
        {
            return new PersonalizedAccountingCardResponse(
                card.Id, card.BirthDate, card.PassportData, card.Citizenship,
                card.GraduatedEducationalInstitution, card.ResidentialAddress,
                card.StudentId, card.PageId
            );
        }

        public static PersonalizedAccountingCard ToEntity(this UpdatePersonalizedAccountingCardRequest request)
        {
            return new PersonalizedAccountingCard
            {
                BirthDate = request.BirthDate,
                PassportData = request.PassportData,
                Citizenship = request.Citizenship,
                GraduatedEducationalInstitution = request.GraduatedEducationalInstitution,
                ResidentialAddress = request.ResidentialAddress,
                StudentId = request.StudentId
            };
        }
    }
}
