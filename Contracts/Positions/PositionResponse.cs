namespace Contracts.Positions
{
    public class PositionResponse(
        int id, string name, bool isDefaultPosition)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;

        public bool IsDefaultPosition { get; set; } = isDefaultPosition;
    }
}
