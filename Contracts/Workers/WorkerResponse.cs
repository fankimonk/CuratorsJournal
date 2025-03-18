using Contracts.Positions;

namespace Contracts.Workers
{
    public class WorkerResponse(
        int id, string firstName, string middleName, string lastName, PositionResponse position, int? userId)
    {
        public int Id { get; set; } = id;

        public string FirstName { get; set; } = firstName;
        public string MiddleName { get; set; } = middleName;
        public string LastName { get; set; } = lastName;

        public PositionResponse Position { get; set; } = position;

        public int? UserId { get; set; } = userId;
    }
}
