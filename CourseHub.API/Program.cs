using CourseHub.Application.Interfaces;
using CourseHub.Application.Interfaces.Repositories;
using CourseHub.Infrastructure;
using CourseHub.Infrastructure.ORM;
using CourseHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CourseHub.Application.DTOs.Students;
using CourseHub.Application.Services;
using CourseHub.Infrastructure.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddScoped<IStudentValidator, RegisterStudentValidator>();
builder.Services.AddScoped<IUpdateStudentValidator, UpdateStudentValidator>();
builder.Services.AddScoped<ICourseValidator, CourseValidator>();
builder.Services.AddScoped<IEnrollmentValidator, EnrollStudentValidator>();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
