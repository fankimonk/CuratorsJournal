using Contracts.ChronicDiseases;
using Domain.Entities;

namespace API.Mappers
{
    public static class ChronicDiseasesMapper
    {
        public static ChronicDiseaseResponse ToResponse(this ChronicDisease position)
        {
            return new ChronicDiseaseResponse(
                position.Id, position.Name
            );
        }

        public static ChronicDisease ToEntity(this CreateChronicDiseaseRequest request)
        {
            return new ChronicDisease
            {
                Name = request.Name
            };
        }

        public static ChronicDisease ToEntity(this UpdateChronicDiseaseRequest request)
        {
            return new ChronicDisease
            {
                Name = request.Name
            };
        }
    }
}
