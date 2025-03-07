using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.ParentalInformation
{
    public class CreateParentalInformationRecordRequest(
        string? firstName, string? middleName, string? lastName,
        string? placeOfResidence, string? placeOfWork, string? position,
        string? homePhoneNumber, string? workPhoneNumber, string? mobilePhoneNumber,
        string? otherInformation, int cardId)
    {
        public string? FirstName { get; set; } = firstName;
        public string? MiddleName { get; set; } = middleName;
        public string? LastName { get; set; } = lastName;

        public string? PlaceOfResidence { get; set; } = placeOfResidence;
        public string? PlaceOfWork { get; set; } = placeOfWork;
        public string? Position { get; set; } = position;

        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        public string? HomePhoneNumber { get; set; } = homePhoneNumber;
        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        public string? WorkPhoneNumber { get; set; } = workPhoneNumber;
        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        public string? MobilePhoneNumber { get; set; } = mobilePhoneNumber;

        public string? OtherInformation { get; set; } = otherInformation;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
