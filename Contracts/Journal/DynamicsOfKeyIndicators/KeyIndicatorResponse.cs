namespace Contracts.Journal.DynamicsOfKeyIndicators
{
    public class KeyIndicatorResponse(int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
