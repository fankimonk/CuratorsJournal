namespace Contracts.Journal.StudentList
{
    public class StudentListPageResponse(int pageId, List<StudentListRecordResponse> studentList)
    {
        public int PageId { get; set; } = pageId;

        public List<StudentListRecordResponse> StudentList { get; set; } = studentList;
    }
}
