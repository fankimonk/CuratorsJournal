using Contracts.Literature;
using Domain.Entities.JournalContent.Literature;

namespace API.Mappers
{
    public static class LiteratureMapper
    {
        public static LiteratureResponse ToResponse(this LiteratureListRecord literature)
        {
            return new LiteratureResponse(literature.Id, literature.Author, literature.Name, literature.BibliographicData);
        }
    }
}
