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

        public static LiteratureListRecord ToEntity(this CreateLiteratureRequest request)
        {
            return new LiteratureListRecord
            {
                Author = request.Author, 
                Name = request.Name, 
                BibliographicData = request.BibliographicData
            };
        }

        public static LiteratureListRecord ToEntity(this UpdateLiteratureRequest request)
        {
            return new LiteratureListRecord
            {
                Author = request.Author,
                Name = request.Name,
                BibliographicData = request.BibliographicData
            };
        }
    }
}
