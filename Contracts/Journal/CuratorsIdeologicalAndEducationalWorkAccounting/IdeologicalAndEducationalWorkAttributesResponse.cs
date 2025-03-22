namespace Contracts.Journal.CuratorsIdeologicalAndEducationalWorkAccounting
{
    public class IdeologicalAndEducationalWorkAttributesResponse(int id, int? month, int? year)
    {
        public int Id { get; set; } = id;
        public int? Month { get; set; } = month;
        public int? Year { get; set; } = year;
    }
}
