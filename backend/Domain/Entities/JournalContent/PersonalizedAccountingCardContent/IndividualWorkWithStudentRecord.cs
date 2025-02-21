using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.JournalContent.PersonalizedAccountingCardContent
{
    [Table("IndividualWorkWithStudent")]
    public class IndividualWorkWithStudentRecord
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public string WorkDoneAndRecommendations { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;

        public int PersonalizedAccountingCardId { get; set; }
        public PersonalizedAccountingCard? PersonalizedAccountingCard { get; set; }
    }
}
