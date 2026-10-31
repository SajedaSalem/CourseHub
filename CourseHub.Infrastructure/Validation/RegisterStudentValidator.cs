using CourseHub.Application.DTOs.Students;
using CourseHub.Application.Exceptions;
using CourseHub.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Infrastructure.Validation
{
    public class RegisterStudentValidator: AbstractValidator<RegisterStudentDto>, IStudentValidator
    {
        public RegisterStudentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Email).NotEmpty().EmailAddress() .MaximumLength(100);

            RuleFor(x => x.DateOfBirth).NotEmpty().LessThan(DateTime.UtcNow.Date);
        }

        public async Task ValidateRegistrationAsync(RegisterStudentDto dto)
        {
            var result = await ValidateAsync(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                        group => group.Key,
                        group => group.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new RegistrationValidationException(errors);
            }
        }
    }
}
