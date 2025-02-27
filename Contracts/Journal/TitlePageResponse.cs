using Contracts.Curator;

namespace Contracts.Journal
{
    public class TitlePageResponse(int journalId, string groupNumber, string admissionYear, 
        CuratorResponse? curator, string departmentName, string facultyName)
    {
        public int JournalId { get; set; } = journalId;
        public string GroupNumber { get; set; } = groupNumber;
        public string AdmissionYear { get; set; } = admissionYear;
        public CuratorResponse? Curator { get; set; } = curator;
        public string DepartmentName { get; set; } = departmentName;
        public string FacultyName { get; set; } = facultyName;
    }
}
