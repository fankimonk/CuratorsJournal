using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    [Table("IndividualInformation")]
    public class IndividualInformationRecord
    {
        public int Id { get; set; }

        public string? ActivityName { get; set; } = string.Empty;

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public string? Result { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;

        public string? ParticipationKind { get; set; }

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
