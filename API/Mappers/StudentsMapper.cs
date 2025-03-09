using Contracts.Students;
using Domain.Entities;

namespace API.Mappers
{
    public static class StudentsMapper
    {
        public static StudentResponse ToResponse(this Student student)
        {
            return new StudentResponse(
                student.Id,
                student.FirstName,
                student.MiddleName,
                student.LastName,
                student.PhoneNumber,
                student.GroupId
            );
        }
    }
}
