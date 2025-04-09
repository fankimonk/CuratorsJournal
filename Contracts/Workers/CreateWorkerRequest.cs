using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Contracts.Workers
{
    public class CreateWorkerRequest(
        string firstName, string middleName, string lastName, int? positionId)
    {
        [Required]
        public string FirstName { get; set; } = firstName;

        [Required]
        public string MiddleName { get; set; } = middleName;

        [Required]
        public string LastName { get; set; } = lastName;

        [Required]
        [NotNull]
        public int? PositionId { get; set; } = positionId;
    }
}
