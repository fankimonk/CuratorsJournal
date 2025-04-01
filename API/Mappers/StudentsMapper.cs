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
                student.GroupId,
                student.UserId
            );
        }

        public static Student ToEntity(this CreateStudentRequest request)
        {
            return new Student
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                GroupId = (int)request.GroupId,
                UserId = request.UserId
            };
        }

        public static Student ToEntity(this UpdateStudentRequest request)
        {
            return new Student
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                GroupId = request.GroupId,
                UserId = request.UserId
            };
        }
    }
}
