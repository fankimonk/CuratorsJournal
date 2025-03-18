namespace Contracts.Positions
{
    public class PositionResponse(
        int id, string? name)
    {
        public int Id { get; set; } = id;

        public string? Name { get; set; } = name;
    }
}
