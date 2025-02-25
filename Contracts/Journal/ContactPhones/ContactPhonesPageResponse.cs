namespace Contracts.Journal.ContactPhones
{
    public record ContactPhonesPageResponse
    (
        int JournalId,
        List<ContactPhoneResponse> PhoneNumbers
    );

    public record ContactPhoneResponse
    (
        int Id,
        string Name,
        string PhoneNumber
    );
}
