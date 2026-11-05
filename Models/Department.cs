using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // One-to-Many: One department has many students
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}