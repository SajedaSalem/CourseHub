using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.DTOs.Enrollments
{
    public class EnrollStudentDto
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }
    }
}
