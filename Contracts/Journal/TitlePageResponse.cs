using Contracts.Curator;

namespace Contracts.Journal
{
    public record TitlePageResponse
    (
        int JournalId,
        string GroupNumber,
        string AdmissionYear,
        CuratorResponse? Curator,
        string DepartmentName,
        string FacultyName
    );
}
