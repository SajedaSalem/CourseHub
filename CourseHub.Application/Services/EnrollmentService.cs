using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Enrollments;
using CourseHub.Application.Interfaces;
using CourseHub.Application.Interfaces.Repositories;
using CourseHub.Domain.Entities;

namespace CourseHub.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnrollmentValidator _validator;

        public EnrollmentService(IStudentRepository studentRepository,IRepository<Course> courseRepository,IEnrollmentRepository enrollmentRepository, IUnitOfWork unitOfWork, IEnrollmentValidator validator)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Guid> EnrollStudentAsync(EnrollStudentDto dto)
        {
            await _validator.ValidateEnrollmentAsync(dto);

            //Check whether student exists
            var student = await _studentRepository.GetByIdAsync(dto.StudentId);

            if (student == null)
                throw new KeyNotFoundException("Student not found.");

            // Check whether course exists
            var course = await _courseRepository.GetByIdAsync(dto.CourseId);

            if (course == null)
                throw new KeyNotFoundException("Course not found.");

            // Prevent duplicate enrollment
            bool alreadyEnrolled = await _enrollmentRepository.ExistsAsync( dto.StudentId,dto.CourseId);

            if (alreadyEnrolled)
                throw new InvalidOperationException("Student is already enrolled in this course.");

            // Check course capacity
            int enrolledCount =await _enrollmentRepository.CountByCourseAsync(dto.CourseId);

            if (enrolledCount >= course.Capacity)
                throw new InvalidOperationException("Course has reached its maximum capacity.");

            // Create enrollment entity
            var enrollment = new Student_Course
            {
                Id = dto.Id,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                EnrolledAt = DateTime.UtcNow
            };

            //Save
            await _enrollmentRepository.AddAsync(enrollment);

            await _unitOfWork.SaveChangesAsync();

            return enrollment.Id;
        }

        public async Task<List<EnrollmentResponseDto>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _enrollmentRepository.GetAllAsync();

            return enrollments.Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrolledAt = e.EnrolledAt
            }).ToList();
        }

        public async Task<bool> DeleteEnrollmentAsync(Guid id)
        {
            var enrollment = await _enrollmentRepository.GetByIdAsync(id);

            if (enrollment == null)
                return false;

            _enrollmentRepository.Delete(enrollment);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
