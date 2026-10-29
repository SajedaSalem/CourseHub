using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Student_Course> Enrollments { get; set; } = new();
    }
}
