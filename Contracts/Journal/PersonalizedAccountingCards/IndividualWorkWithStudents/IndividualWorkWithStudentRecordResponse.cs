namespace Contracts.Journal.PersonalizedAccountingCards.IndividualWorkWithStudents
{
    public class IndividualWorkWithStudentRecordResponse(
        int id, DateOnly? date, string? workDoneAndRecommendations, string? result, int cardId)
    {
        public int Id { get; set; } = id;

        public DateOnly? Date { get; set; } = date;

        public string? WorkDoneAndRecommendations { get; set; } = workDoneAndRecommendations;
        public string? Result { get; set; } = result;

        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
