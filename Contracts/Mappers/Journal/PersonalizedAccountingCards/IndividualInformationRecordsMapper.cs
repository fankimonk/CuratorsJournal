using Contracts.Journal.PersonalizedAccountingCards.IndividualInformation;

namespace Contracts.Mappers.Journal.PersonalizedAccountingCards
{
    public static class IndividualInformationRecordsMapper
    {
        public static UpdateIndividualInformationRecordRequest ToRequest(this IndividualInformationRecordResponse record)
        {
            return new UpdateIndividualInformationRecordRequest(
                record.ActivityName,
                record.StartDate,
                record.EndDate,
                record.Result,
                record.Note,
                record.ActivityTypeId
            );
        }
    }
}
