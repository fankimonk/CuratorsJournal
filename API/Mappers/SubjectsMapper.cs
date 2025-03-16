using Contracts.Subjects;
using Domain.Entities;

namespace API.Mappers
{
    public static class SubjectsMapper
    {
        public static SubjectResponse ToResponse(this Subject subject)
        {
            return new SubjectResponse(
                subject.Id, subject.Name, subject.AbbreviatedName
            );
        }

        public static Subject ToEntity(this CreateSubjectRequest request)
        {
            return new Subject
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName
            };
        }

        public static Subject ToEntity(this UpdateSubjectRequest request)
        {
            return new Subject
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName
            };
        }
    }
}
