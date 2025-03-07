using Contracts.Journal.PersonalizedAccountingCards;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class PersonalizedAccountingCardsMapper
    {
        public static UpdatePersonalizedAccountingCardRequest ToRequest(this PersonalizedAccountingCardResponse record)
        {
            return new UpdatePersonalizedAccountingCardRequest(
                record.BirthDate, record.PassportData, record.Citizenship,
                record.GraduatedEducationalInstitution, record.ResidentialAddress,
                record.StudentId
            );
        }
    }
}
