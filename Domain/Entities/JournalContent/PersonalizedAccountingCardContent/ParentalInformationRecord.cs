using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    [Table("ParentalInformation")]
    public class ParentalInformationRecord
    {
        public int Id { get; set; }

        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;

        public string? PlaceOfResidence { get; set; } = string.Empty;
        public string? PlaceOfWork { get; set; } = string.Empty;
        public string? Position { get; set; } = string.Empty;

        public string? HomePhoneNumber { get; set; } = string.Empty;
        public string? WorkPhoneNumber { get; set; } = string.Empty;
        public string? MobilePhoneNumber { get; set; } = string.Empty;

        public string? OtherInformation { get; set; } = string.Empty;

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
