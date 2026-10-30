using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CourseHub.Infrastructure.ORM.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Title).IsRequired().HasMaxLength(100);

            builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);

            builder.Property(c => c.Capacity).IsRequired();



        }
    }
}
