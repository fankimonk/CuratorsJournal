using System.ComponentModel.DataAnnotations;

namespace Contracts.Workers
{
    public class CreateWorkerRequest(
        string firstName, string middleName, string lastName, int positionId, int? userId)
    {
        [Required]
        public string FirstName { get; set; } = firstName;

        [Required]
        public string MiddleName { get; set; } = middleName;

        [Required]
        public string LastName { get; set; } = lastName;

        [Required]
        public int PositionId { get; set; } = positionId;

        public int? UserId { get; set; } = userId;
    }
}
