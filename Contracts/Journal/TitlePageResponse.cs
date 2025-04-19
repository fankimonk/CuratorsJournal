using Contracts.Curator;
using Contracts.Groups;

namespace Contracts.Journal
{
    public class TitlePageResponse(int pageId, int journalId, GroupResponse group, 
        CuratorResponse? curator, string departmentName, string facultyName)
    {
        public int PageId { get; set; } = pageId;
        public int JournalId { get; set; } = journalId;
        public GroupResponse Group { get; set; } = group;
        public CuratorResponse? Curator { get; set; } = curator;
        public string DepartmentName { get; set; } = departmentName;
        public string FacultyName { get; set; } = facultyName;
    }
}
