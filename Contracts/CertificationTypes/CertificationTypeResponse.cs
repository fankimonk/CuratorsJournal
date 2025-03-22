namespace Contracts.CertificationTypes
{
    public class CertificationTypeResponse(
        int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
