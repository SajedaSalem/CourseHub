using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.DTOs.Courses
{
    public class CourseResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
