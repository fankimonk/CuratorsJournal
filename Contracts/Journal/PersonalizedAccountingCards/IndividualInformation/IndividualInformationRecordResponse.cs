namespace Contracts.Journal.PersonalizedAccountingCards.IndividualInformation
{
    public class IndividualInformationRecordResponse(
        int id, string? activityName, DateOnly? startDate, DateOnly? endDate,
        string? result, string? note, string? participationKind, int personalizedAccountingCardId)
    {
        public int Id { get; set; } = id;

        public string? ActivityName { get; set; } = activityName;

        public DateOnly? StartDate { get; set; } = startDate;
        public DateOnly? EndDate { get; set; } = endDate;

        public string? Result { get; set; } = result;
        public string? Note { get; set; } = note;

        public string? ParticipationKind { get; set; } = participationKind;

        public int PersonalizedAccountingCardId { get; set; } = personalizedAccountingCardId;
    }
}
