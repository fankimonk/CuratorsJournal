namespace Contracts.Journal.PersonalizedAccountingCards.ParentalInformation
{
    public class ParentalInformationRecordResponse(
        int id, string? firstName, string? middleName, string? lastName,
        string? placeOfResidence, string? placeOfWork, string? position,
        string? homePhoneNumber, string? workPhoneNumber, string? mobilePhoneNumber,
        string? otherInformation, int cardId)
    {
        public int Id { get; set; } = id;

        public string? FirstName { get; set; } = firstName;
        public string? MiddleName { get; set; } = middleName;
        public string? LastName { get; set; } = lastName;

        public string? PlaceOfResidence { get; set; } = placeOfResidence;
        public string? PlaceOfWork { get; set; } = placeOfWork;
        public string? Position { get; set; } = position;

        public string? HomePhoneNumber { get; set; } = homePhoneNumber;
        public string? WorkPhoneNumber { get; set; } = workPhoneNumber;
        public string? MobilePhoneNumber { get; set; } = mobilePhoneNumber;

        public string? OtherInformation { get; set; } = otherInformation;

        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
