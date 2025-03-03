namespace Contracts.Journal.GroupActives
{
    public class GroupActiveResponse(int id, string positionName, int studentId)
    {
        public int Id { get; set; } = id;

        public string PositionName { get; set; } = positionName;

        public int StudentId { get; set; } = studentId;
    }
}
