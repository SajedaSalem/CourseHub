using System;
using System.Collections.Generic;
using System.Text;
using CourseHub.Application.Interfaces;
using CourseHub.Infrastructure.ORM;

namespace CourseHub.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
