using Contracts.Journal.PersonalizedAccountingCards.ParentalInformation;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class ParentalInformationRecordsMapper
    {
        public static UpdateParentalInformationRecordRequest ToRequest(this ParentalInformationRecordResponse record)
        {
            return new UpdateParentalInformationRecordRequest(
                record.FirstName, record.MiddleName, record.LastName,
                record.PlaceOfResidence, record.PlaceOfResidence, record.Position,
                record.HomePhoneNumber, record.WorkPhoneNumber, record.MobilePhoneNumber,
                record.OtherInformation
            );
        }
    }
}
