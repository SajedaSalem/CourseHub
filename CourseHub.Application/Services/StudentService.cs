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

        public StudentService( IStudentRepository studentRepository,IUnitOfWork unitOfWork, IStudentValidator validator)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
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
    }
}
