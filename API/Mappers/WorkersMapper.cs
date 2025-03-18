using Contracts.Positions;
using Contracts.Workers;
using Domain.Entities;

namespace API.Mappers
{
    public static class WorkersMapper
    {
        public static WorkerResponse ToResponse(this Worker worker)
        {
            return new WorkerResponse(
                worker.Id,
                worker.FirstName,
                worker.MiddleName,
                worker.LastName,
                worker.Position == null ? new PositionResponse(worker.PositionId, null) : worker.Position.ToResponse(),
                worker.UserId
            );
        }

        public static Worker ToEntity(this CreateWorkerRequest request)
        {
            return new Worker
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PositionId = (int)request.PositionId,
                UserId = request.UserId
            };
        }

        public static Worker ToEntity(this UpdateWorkerRequest request)
        {
            return new Worker
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PositionId = (int)request.PositionId,
                UserId = request.UserId
            };
        }
    }
}
