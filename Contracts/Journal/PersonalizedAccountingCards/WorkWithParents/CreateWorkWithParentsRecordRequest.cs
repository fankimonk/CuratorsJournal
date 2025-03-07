using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.WorkWithParents
{
    public class CreateWorkWithParentsRecordRequest(
        DateOnly? date, string? workContent, string? note, int cardId)
    {
        public DateOnly? Date { get; set; } = date;

        public string? WorkContent { get; set; } = workContent;
        public string? Note { get; set; } = note;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
