namespace Contracts.Literature
{
    public class LiteratureResponse
    (
        int id,
        string author,
        string name,
        string bibliographicData
    )
    {
        public int Id { get; set; } = id;

        public string Author { get; set; } = author;
        public string Name { get; set; } = name;
        public string BibliographicData { get; set; } = bibliographicData;
    }
}
