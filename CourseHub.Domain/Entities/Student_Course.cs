using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Domain.Entities
{
    public class Student_Course
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; } 
        public Course Course { get; set; }
    }
}
