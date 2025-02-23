using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    [Table("WorkWithParents")]
    public class WorkWithParentsRecord
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string WorkContent { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
