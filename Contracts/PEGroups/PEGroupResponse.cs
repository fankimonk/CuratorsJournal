namespace Contracts.PEGroups
{
    public class PEGroupResponse(
        int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
