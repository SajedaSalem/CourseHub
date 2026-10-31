using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Courses;
using CourseHub.Application.Interfaces;
using CourseHub.Application.Interfaces.Repositories;
using CourseHub.Domain.Entities;

namespace CourseHub.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseValidator _validator;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseService(IRepository<Course> courseRepository,IUnitOfWork unitOfWork,ICourseValidator validator, IEnrollmentRepository enrollmentRepository)
        {
            _courseRepository = courseRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Guid> CreateCourseAsync(CreateCourseDto dto)
        {
            await _validator.ValidateCreateAsync(dto);

            var existingCourse = await _courseRepository.GetByIdAsync(dto.Id);

            if (existingCourse != null)
                throw new InvalidOperationException("A course with this ID already exists.");

            var course = new Course
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Capacity = dto.Capacity,
                CreatedAt = DateTime.UtcNow
            };

            await _courseRepository.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return course.Id;
        }

        public async Task<List<CourseResponseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();

            return courses.Select(MapToDto).ToList();
        }

        public async Task<CourseResponseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await _courseRepository.GetByIdAsync(id);

            return course == null ? null : MapToDto(course);
        }

        public async Task<bool> UpdateCourseAsync( Guid id,UpdateCourseDto dto)
        {
            await _validator.ValidateUpdateAsync(dto);

            var course = await _courseRepository.GetByIdAsync(id);

            if (course == null)
                return false;

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Capacity = dto.Capacity;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _courseRepository.GetByIdAsync(id);

            if (course == null)
                return false;

            int enrollmentCount = await _enrollmentRepository.CountByCourseAsync(id);

            if (enrollmentCount > 0)
            {
                throw new InvalidOperationException("Cannot delete course because students are enrolled in it.");
            }

            _courseRepository.Delete(course);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static CourseResponseDto MapToDto(Course course)
        {
            return new CourseResponseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Capacity = course.Capacity,
                CreatedAt = course.CreatedAt
            };
        }

    }
}
