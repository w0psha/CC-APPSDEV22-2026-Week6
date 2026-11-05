using StudentRosterDbApi.DTOs;

namespace StudentRosterDbApi.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> CreateStudentAsync(StudentCreateDto dto);
        Task<bool> DeleteStudentAsync(int id); // Requirement for Attempt 2
    }
}