using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Enrollments;
using CourseHub.Application.Exceptions;
using CourseHub.Application.Interfaces;
using FluentValidation;

namespace CourseHub.Infrastructure.Validation
{
    public class EnrollStudentValidator : AbstractValidator<EnrollStudentDto>, IEnrollmentValidator
    {
        public EnrollStudentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.StudentId).NotEmpty();
            RuleFor(x => x.CourseId).NotEmpty();
        }

        public async Task ValidateEnrollmentAsync(EnrollStudentDto dto)
        {
            var result = await ValidateAsync(dto);

            if (!result.IsValid)
            {
                var errors = result.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                        group => group.Key,
                        group => group.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new ApplicationValidationException(errors);
            }
        }
    }
}
