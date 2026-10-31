using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.DTOs.Students
{
    public class StudentResponseDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
