using System;
using System.Collections.Generic;
using System.Text;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.EF.Data;
namespace HelpDeskTickets.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IBaseRepository<Ticket> Tickets { get; private set; }
        public IBaseRepository<Comment> Comments { get; private set; }
        public IBaseRepository<Department> Departments { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Tickets = new BaseRepository<Ticket>(_context);
            Comments = new BaseRepository<Comment>(_context);
            Departments = new BaseRepository<Department>(_context);

        }

        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
