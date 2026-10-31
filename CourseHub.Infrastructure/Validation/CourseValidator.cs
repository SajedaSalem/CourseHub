using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Courses;
using CourseHub.Application.Exceptions;
using CourseHub.Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;


namespace CourseHub.Infrastructure.Validation
{
    public class CourseValidator : ICourseValidator
    {
        private readonly CreateCourseValidator _createValidator = new();

        private readonly UpdateCourseValidator _updateValidator = new();

        public async Task ValidateCreateAsync(CreateCourseDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);

            ThrowIfInvalid(result);
        }

        public async Task ValidateUpdateAsync(UpdateCourseDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);

            ThrowIfInvalid(result);
        }

        private static void ThrowIfInvalid(ValidationResult result)
        {
            if (result.IsValid)
                return;

            var errors = result.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToArray());

            throw new ApplicationValidationException(errors);
        }
    }
}
