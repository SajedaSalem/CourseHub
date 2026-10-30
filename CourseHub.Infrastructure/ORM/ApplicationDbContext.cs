using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.ORM.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CourseHub.Infrastructure.ORM
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student_Course> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new StudentCourseConfiguration());
        }


    }
}