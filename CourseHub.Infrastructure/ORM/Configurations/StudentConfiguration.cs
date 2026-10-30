using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Infrastructure.ORM.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).ValueGeneratedNever();

            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(100);

            builder.Property(s => s.LastName).IsRequired().HasMaxLength(100);

            builder.Property(s => s.Email).IsRequired().HasMaxLength(100);

            builder.HasIndex(s => s.Email).IsUnique();
        }
    }
}
