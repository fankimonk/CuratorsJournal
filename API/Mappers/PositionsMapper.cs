using Contracts.Positions;
using Domain.Entities;

namespace API.Mappers
{
    public static class PositionsMapper
    {
        public static PositionResponse ToResponse(this Position position)
        {
            return new PositionResponse(
                position.Id, position.Name, position.IsDefaultPosition
            );
        }

        public static Position ToEntity(this CreatePositionRequest request)
        {
            return new Position
            {
                Name = request.Name
            };
        }

        public static Position ToEntity(this UpdatePositionRequest request)
        {
            return new Position
            {
                Name = request.Name
            };
        }
    }
}
