using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.ParentalInformation
{
    public record UpdateParentalInformationRecordRequest
    (
        string? FirstName,
        string? MiddleName,
        string? LastName,
        
        string? PlaceOfResidence,
        string? PlaceOfWork,
        string? Position,

        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        string? HomePhoneNumber,
        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        string? WorkPhoneNumber,
        [Phone]
        [MinLength(7)]
        [MaxLength(19)]
        string? MobilePhoneNumber,
        
        string? OtherInformation
    );
}
