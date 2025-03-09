using Contracts.Positions;
using Domain.Entities;

namespace API.Mappers
{
    public static class PositionsMapper
    {
        public static PositionResponse ToResponse(this Position position)
        {
            return new PositionResponse(
                position.Id, position.Name
            );
        }
    }
}
