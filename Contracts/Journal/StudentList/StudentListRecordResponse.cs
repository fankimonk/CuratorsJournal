namespace Contracts.Journal.StudentList
{
    public class StudentListRecordResponse(int id, int number, int studentId, int? cardId)
    {
        public int Id { get; set; } = id;

        public int Number { get; set; } = number;
        public int StudentId { get; set; } = studentId;

        public int? PersonalizedAccountingCardId { get; set; } = cardId;
    }

    public class StudentListPageResponse(int pageId, List<StudentListRecordResponse> studentList)
    {
        public int PageId { get; set; } = pageId;

        public List<StudentListRecordResponse> StudentList { get; set; } = studentList;
    }
}
