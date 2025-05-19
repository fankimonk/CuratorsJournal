namespace Frontend.Utils.Filters
{
    public class StudentFilter(string lastName, int? groupId)
    {
        public string LastName { get; set; } = lastName;
        public int? GroupId { get; set; } = groupId;
    }
}
