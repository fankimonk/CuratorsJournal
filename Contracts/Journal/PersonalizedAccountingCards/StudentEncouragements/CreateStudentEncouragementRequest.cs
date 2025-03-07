using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements
{
    public class CreateStudentEncouragementRequest(
        DateOnly? date, string? achievement, string? encouragementKind, int cardId)
    {
        public DateOnly? Date { get; set; } = date;

        public string? Achievement { get; set; } = achievement;
        public string? EncouragementKind { get; set; } = encouragementKind;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
