namespace Contracts.ChronicDiseases
{
    public class ChronicDiseaseResponse(
        int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
