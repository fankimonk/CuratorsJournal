using Contracts.Journal.ContactPhones;
using Contracts.Journal.PsychologicalAndPedagogicalCharacteristics;
using Domain.Entities.JournalContent;

namespace API.Mappers
{
    public static class PsychologicalAndPedagogicalCharacteristicsMapper
    {
        public static PsychologicalAndPedagogicalCharacteristicsResponse ToResponse(
            this PsychologicalAndPedagogicalCharacteristics characteristics)
        {
            return new PsychologicalAndPedagogicalCharacteristicsResponse(
                characteristics.Id, characteristics.Content, characteristics.Date, 
                characteristics.WorkerId, characteristics.PageId
            );
        }

        public static PsychologicalAndPedagogicalCharacteristics ToEntity(
            this UpdatePsychologicalAndPedagogicalCharacteristicsRequest request)
        {
            return new PsychologicalAndPedagogicalCharacteristics
            {
                Content = request.Content,
                Date = request.Date,
                WorkerId = request.WorkerId
            };
        }

        public static PsychologicalAndPedagogicalCharacteristics ToEntity(
            this CreatePsychologicalAndPedagogicalCharacteristicsRequest request)
        {
            return new PsychologicalAndPedagogicalCharacteristics
            {
                Content = request.Content,
                Date = request.Date,
                WorkerId = request.WorkerId,
                PageId = request.PageId
            };
        }
    }
}
