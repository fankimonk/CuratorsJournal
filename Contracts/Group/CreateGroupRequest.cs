namespace Contracts.Group
{
    public record CreateGroupRequest
    (
        string Number,
        int AdmissionYear,
        int SpecialtyId
    );
}
