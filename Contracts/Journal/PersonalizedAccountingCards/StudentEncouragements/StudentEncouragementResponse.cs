namespace Contracts.Journal.PersonalizedAccountingCards.StudentEncouragements
{
    public class StudentEncouragementResponse(
        int id, DateOnly? date, string? achievement, string? encouragementKind, int cardId)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;

        public string? Achievement { get; set; } = achievement;
        public string? EncouragementKind { get; set; } = encouragementKind;

        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
