using CourseHub.Application.DTOs.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Interfaces
{
    public interface ICourseValidator
    {
        Task ValidateCreateAsync(CreateCourseDto dto);

        Task ValidateUpdateAsync(UpdateCourseDto dto);
    }
}
