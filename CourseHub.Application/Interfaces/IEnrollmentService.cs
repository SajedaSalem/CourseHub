using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Enrollments;


namespace CourseHub.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Guid> EnrollStudentAsync(EnrollStudentDto dto);

        Task<List<EnrollmentResponseDto>> GetAllEnrollmentsAsync();
    }
}
