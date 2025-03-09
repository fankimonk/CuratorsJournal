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
                worker.Position!.ToResponse()
            );
        }
    }
}
