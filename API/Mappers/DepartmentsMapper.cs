using Contracts.Departments;
using Domain.Entities;

namespace API.Mappers
{
    public static class DepartmentsMapper
    {
        public static DepartmentResponse ToResponse(this Department position)
        {
            return new DepartmentResponse(
                position.Id, position.Name, position.AbbreviatedName, position.HeadId, position.DeaneryId
            );
        }

        public static Department ToEntity(this CreateDepartmentRequest request)
        {
            return new Department
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName,
                HeadId = request.HeadId,
                DeaneryId = request.DeaneryId
            };
        }

        public static Department ToEntity(this UpdateDepartmentRequest request)
        {
            return new Department
            {
                Name = request.Name,
                AbbreviatedName = request.AbbreviatedName,
                HeadId = request.HeadId,
                DeaneryId = request.DeaneryId
            };
        }
    }
}
