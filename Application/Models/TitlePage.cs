namespace Application.Models
{
    public class TitlePage : Page
    {
        public string GroupNumber { get; private set; }
        public string DepartmentName { get; private set; }
        public string FacultyName { get; private set; }

        public int AdmissionYear { get; private set; }

        public Dictionary<(string, string, string), DateOnly> Curators { get; private set; }

        public TitlePage(string title, string groupNumber, int admissionYear, Dictionary<(string, string, string), DateOnly> curators,
            string departmentName, string facultyName) : base(title)
        {
            GroupNumber = groupNumber;
            DepartmentName = departmentName;
            FacultyName = facultyName;
            AdmissionYear = admissionYear;
            Curators = curators;
        }
    }
}
