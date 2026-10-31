using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Enrollments;

namespace CourseHub.Application.Interfaces
{
    public interface IEnrollmentValidator
    {
        Task ValidateEnrollmentAsync(EnrollStudentDto dto);

    }
}
