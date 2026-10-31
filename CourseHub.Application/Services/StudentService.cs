using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Students;
using CourseHub.Application.Interfaces;
using CourseHub.Application.Interfaces.Repositories;
using CourseHub.Domain.Entities;

namespace CourseHub.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentValidator _validator;

        private readonly IUpdateStudentValidator _updateValidator;

        public StudentService( IStudentRepository studentRepository,IUnitOfWork unitOfWork, IStudentValidator validator, IUpdateStudentValidator updateValidator)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        public async Task<Guid> RegisterStudentAsync(RegisterStudentDto dto)
        {
            // Validate the incoming DTO
            await _validator.ValidateRegistrationAsync(dto);

            //  Check whether the ID already exists
            var existingStudent = await _studentRepository.GetByIdAsync(dto.Id);

            if (existingStudent != null)
            {
                throw new InvalidOperationException( "A student with this ID already exists.");
            }

            // Check whether the email already exists
            bool emailExists =  await _studentRepository.ExistsByEmailAsync(dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException("A student with this email already exists.");
            }

            //  DTO to Domain entity
            var student = new Student
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth
            };

            // Add student to EF Core change tracker
            await _studentRepository.AddAsync(student);

            //  Save changes to SQL Server
            await _unitOfWork.SaveChangesAsync();

            return student.Id;


        }

        public async Task<List<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();

            return students.Select(s => new StudentResponseDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth,
                RegisteredAt = s.RegisteredAt
            }).ToList();
        }

        public async Task<StudentResponseDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return null;
            }

            return new StudentResponseDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                RegisteredAt = student.RegisteredAt
            };
        }

        public async Task<bool> UpdateStudentAsync(Guid id, UpdateStudentDto dto)
        {
            // Validate incoming data
            await _updateValidator.ValidateUpdateAsync(dto);

            // Find student
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
                return false;

            // Check email uniqueness
            if (student.Email != dto.Email)
            {
                bool emailExists =
                    await _studentRepository.ExistsByEmailAsync(dto.Email);

                if (emailExists)
                {
                    throw new InvalidOperationException(
                        "A student with this email already exists.");
                }
            }

            // Update properties
            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.DateOfBirth = dto.DateOfBirth;

            // Persist changes
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
                return false;

            _studentRepository.Delete(student);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
