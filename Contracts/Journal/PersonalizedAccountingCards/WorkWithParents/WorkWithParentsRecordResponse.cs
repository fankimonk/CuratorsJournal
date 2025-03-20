namespace Contracts.Journal.PersonalizedAccountingCards.WorkWithParents
{
    public class WorkWithParentsRecordResponse(
        int id, DateOnly? date, string? workContent, string? note, int personalizedAccountingCardId)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;

        public string? WorkContent { get; set; } = workContent;
        public string? Note { get; set; } = note;

        public int PersonalizedAccountingCardId { get; set; } = personalizedAccountingCardId;
    }
}
