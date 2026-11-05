using Microsoft.EntityFrameworkCore;
using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}