using System;
using System.Collections.Generic;
using System.Text;

namespace CourseHub.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

    }
}
