namespace Contracts.ActivityTypes
{
    public class ActivityTypeResponse(
        int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
