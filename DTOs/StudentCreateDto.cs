using System.ComponentModel.DataAnnotations;

namespace StudentRosterDbApi.DTOs
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Year level must be between 1 and 6.")]
        public int YearLevel { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}