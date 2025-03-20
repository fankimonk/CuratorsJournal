using System.ComponentModel.DataAnnotations;

namespace Contracts.Journal.PersonalizedAccountingCards.IndividualWorkWithStudents
{
    public class CreateIndividualWorkWithStudentRecordRequest(
        DateOnly? date, string? workDoneAndRecommendations, string? result, int personalizedAccountingCardId)
    {
        public DateOnly? Date { get; set; } = date;

        public string? WorkDoneAndRecommendations { get; set; } = workDoneAndRecommendations;
        public string? Result { get; set; } = result;

        [Required]
        public int PersonalizedAccountingCardId { get; set; } = personalizedAccountingCardId;
    }
}
