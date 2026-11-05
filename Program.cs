using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentRosterDbApi.Data;
using StudentRosterDbApi.Models;
using StudentRosterDbApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=studentroster.db"));

// Register AutoMapper & Custom Services
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IStudentService, StudentService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "StudentRosterApi",
            ValidAudience = "StudentRosterClient",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsAVeryLongAndSecureSecretKeyForLabPurposesOnly123!"))
        };
    });

var app = builder.Build();

// Configure Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Handler (Attempt 2 Requirement)
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            StatusCode = 500,
            Message = "An internal server error occurred while processing your request."
        });
    });
});

app.UseHttpsRedirection();

// Middleware Order Critical Step: Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Default Data (Prevents foreign key issues)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Departments.Any())
    {
        context.Departments.Add(new Department { Name = "Computer Science" });
        context.Courses.Add(new Course { Title = "Applications Development" });
        context.SaveChanges();
    }
}

app.Run();