namespace StudentRosterDbApi.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
    }
}