using HelpDeskTickets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.EF.Repositories
{
    public class BaseRepository<T> :IBaseRepository <T> where T : class
    {
        protected AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
           _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            
            return entity;
        }
    }
}
