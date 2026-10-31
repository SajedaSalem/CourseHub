using CourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Enrollments;
using CourseHub.Application.Interfaces;
using CourseHub.Application.Interfaces.Repositories;

namespace CourseHub.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository : IRepository<Student_Course>
    {
        Task<bool> ExistsAsync(Guid studentId, Guid courseId);

        Task<int> CountByCourseAsync(Guid courseId);
    }
}
