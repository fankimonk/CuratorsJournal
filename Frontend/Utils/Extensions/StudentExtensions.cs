using Contracts.Students;

namespace Frontend.Utils.Extensions
{
    public static class StudentExtensions
    {
        public static string GetFIO(this StudentResponse student)
        {
            return student.LastName + " " + student.FirstName + " " + student.MiddleName;
        }
    }
}
