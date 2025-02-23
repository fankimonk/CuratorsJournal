namespace API.Contracts.Journal
{
    public record TitlePageResponse
    (
        int JournalId,
        string GroupNumber,
        string AdmissionYear,
        (string, string, string)? CuratorFIO,
        string DepartmentName,
        string FacultyName
    );
}
