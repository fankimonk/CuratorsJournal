namespace Contracts.Journal.StudentList
{
    public class StudentListRecordResponse(int id, int? number, int? studentId, CardInfoResponse? cardInfo)
    {
        public int Id { get; set; } = id;

        public int? Number { get; set; } = number;
        public int? StudentId { get; set; } = studentId;

        public CardInfoResponse? CardInfo { get; set; } = cardInfo;
    }
}
