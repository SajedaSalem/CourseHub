using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.Interfaces.Repositories;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.Repositories
{
    public class EnrollmentRepository : Repository<Student_Course>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(Guid studentId, Guid courseId)
        {
            return await _context.StudentCourses.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }

        public async Task<int> CountByCourseAsync(Guid courseId)
        {
            return await _context.StudentCourses.CountAsync(e => e.CourseId == courseId);
        }

    }
}
