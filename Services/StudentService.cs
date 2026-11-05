using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentRosterDbApi.Data;
using StudentRosterDbApi.DTOs;
using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _context.Students
                .Include(s => s.Department) // Eager loading
                .ToListAsync();

            return _mapper.Map<List<StudentDto>>(students);
        }

        public async Task<StudentDto> CreateStudentAsync(StudentCreateDto dto)
        {
            // 1. Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. Map DTO to Entity
                var student = _mapper.Map<Student>(dto);
                _context.Students.Add(student);

                // 3. Create Audit Log
                var auditLog = new AuditLog
                {
                    Action = $"Created student: {student.FirstName} {student.LastName}"
                };
                _context.AuditLogs.Add(auditLog);

                // 4. Save Changes & Commit Transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<StudentDto>(student);
            }
            catch (Exception)
            {
                // 5. Rollback on Exception
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Implementation for Attempt 2 Completion: Delete with Transaction
        public async Task<bool> DeleteStudentAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return false;
                }

                _context.Students.Remove(student);

                var auditLog = new AuditLog
                {
                    Action = $"Deleted student with ID: {id}"
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}