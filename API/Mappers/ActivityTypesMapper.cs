using Contracts.ActivityTypes;
using Domain.Entities;

namespace API.Mappers
{
    public static class ActivityTypesMapper
    {
        public static ActivityTypeResponse ToResponse(this ActivityType position)
        {
            return new ActivityTypeResponse(
                position.Id, position.Name
            );
        }

        public static ActivityType ToEntity(this CreateActivityTypeRequest request)
        {
            return new ActivityType
            {
                Name = request.Name
            };
        }

        public static ActivityType ToEntity(this UpdateActivityTypeRequest request)
        {
            return new ActivityType
            {
                Name = request.Name
            };
        }
    }
}
