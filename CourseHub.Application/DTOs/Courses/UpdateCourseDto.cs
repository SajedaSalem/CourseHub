using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.DTOs.Courses
{
    public class UpdateCourseDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }
    }
}
