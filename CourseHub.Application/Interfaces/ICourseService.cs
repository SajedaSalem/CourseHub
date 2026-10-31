using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Courses;


namespace CourseHub.Application.Interfaces
{
    public interface ICourseService
    {
        Task<Guid> CreateCourseAsync(CreateCourseDto dto);

        Task<List<CourseResponseDto>> GetAllCoursesAsync();

        Task<CourseResponseDto?> GetCourseByIdAsync(Guid id);

        Task<bool> UpdateCourseAsync(Guid id, UpdateCourseDto dto);

        Task<bool> DeleteCourseAsync(Guid id);
    }
}
