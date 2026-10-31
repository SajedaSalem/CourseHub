using CourseHub.Application.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Interfaces
{
    public interface IStudentService
    {
        Task<Guid> RegisterStudentAsync(RegisterStudentDto dto);
        Task<List<StudentResponseDto>> GetAllStudentsAsync();
        Task<StudentResponseDto?> GetStudentByIdAsync(Guid id);

    }
}
