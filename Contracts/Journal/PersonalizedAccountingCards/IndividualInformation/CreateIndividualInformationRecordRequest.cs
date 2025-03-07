using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.IndividualInformation
{
    public class CreateIndividualInformationRecordRequest(
        string? activityName, DateOnly? startDate, DateOnly? endDate,
        string? result, string? note, int? activityTypeId, int cardId)
    {
        public string? ActivityName { get; set; } = activityName;

        public DateOnly? StartDate { get; set; } = startDate;
        public DateOnly? EndDate { get; set; } = endDate;

        public string? Result { get; set; } = result;
        public string? Note { get; set; } = note;

        public int? ActivityTypeId { get; set; } = activityTypeId;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = cardId;
    }
}
