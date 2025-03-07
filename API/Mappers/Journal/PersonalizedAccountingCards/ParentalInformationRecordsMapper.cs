using Contracts.Journal.PersonalizedAccountingCards.ParentalInformation;
using Domain.Entities.JournalContent.PersonalizedAccountingCardContent;

namespace API.Mappers.Journal.PersonalizedAccountingCards
{
    public static class ParentalInformationRecordsMapper
    {
        public static ParentalInformationRecordResponse ToResponse(this ParentalInformationRecord record)
        {
            return new ParentalInformationRecordResponse(
                record.Id,
                record.FirstName,
                record.MiddleName,
                record.LastName,
                record.PlaceOfResidence,
                record.PlaceOfWork,
                record.Position,
                record.HomePhoneNumber,
                record.WorkPhoneNumber,
                record.MobilePhoneNumber,
                record.OtherInformation,
                record.PersonalizedAccountingCardId
            );
        }

        public static ParentalInformationRecord ToEntity(this UpdateParentalInformationRecordRequest request)
        {
            return new ParentalInformationRecord
            {
               FirstName = request.FirstName,
               MiddleName = request.MiddleName,
               LastName = request.LastName,
               PlaceOfResidence = request.PlaceOfResidence,
               PlaceOfWork = request.PlaceOfWork,
               Position = request.Position,
               HomePhoneNumber = request.HomePhoneNumber,
               WorkPhoneNumber = request.WorkPhoneNumber,
               MobilePhoneNumber = request.MobilePhoneNumber,
               OtherInformation = request.OtherInformation
            };
        }

        public static ParentalInformationRecord ToEntity(this CreateParentalInformationRecordRequest request)
        {
            return new ParentalInformationRecord
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PlaceOfResidence = request.PlaceOfResidence,
                PlaceOfWork = request.PlaceOfWork,
                Position = request.Position,
                HomePhoneNumber = request.HomePhoneNumber,
                WorkPhoneNumber = request.WorkPhoneNumber,
                MobilePhoneNumber = request.MobilePhoneNumber,
                OtherInformation = request.OtherInformation,
                PersonalizedAccountingCardId = request.PersonalizedAccountingCardId
            };
        }
    }
}
