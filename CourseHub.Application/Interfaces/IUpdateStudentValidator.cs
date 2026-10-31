using CourseHub.Application.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Interfaces
{
    public interface IUpdateStudentValidator
    {
        Task ValidateUpdateAsync(UpdateStudentDto dto);

    }
}
