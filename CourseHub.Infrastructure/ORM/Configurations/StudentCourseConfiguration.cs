using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CourseHub.Infrastructure.ORM.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<Student_Course>
    {
        public void Configure(EntityTypeBuilder<Student_Course> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Id).ValueGeneratedNever();

            builder.HasOne(sc => sc.Student).WithMany(s => s.Enrollments).HasForeignKey(sc => sc.StudentId);

            builder.HasOne(sc => sc.Course).WithMany(c => c.Enrollments).HasForeignKey(sc => sc.CourseId);

            builder.HasIndex(sc => new { sc.StudentId, sc.CourseId }).IsUnique();
        }
    }
}
