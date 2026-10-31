using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.DTOs.Courses;
using FluentValidation;

namespace CourseHub.Infrastructure.Validation
{
    public class UpdateCourseValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);

            RuleFor(x => x.Capacity) .GreaterThan(0);
        }
    }
}
