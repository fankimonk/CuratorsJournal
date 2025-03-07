using Contracts.Journal.PsychologicalAndPedagogicalCharacteristics;

namespace Contracts.Mappers.Journal
{
    public static class PsychologicalAndPedagogicalCharacteristicsMapper
    {
        public static UpdatePsychologicalAndPedagogicalCharacteristicsRequest ToRequest(this PsychologicalAndPedagogicalCharacteristicsResponse record)
        {
            return new UpdatePsychologicalAndPedagogicalCharacteristicsRequest(
                record.Content, record.Date, record.WorkerId
            );
        }
    }
}
