using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Domain.Entities;

namespace CourseHub.Application.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
       Task<bool> ExistsByEmailAsync(string email);
    }
}
