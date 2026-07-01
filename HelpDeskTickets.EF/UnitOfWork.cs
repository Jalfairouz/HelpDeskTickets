using System;
using System.Collections.Generic;
using System.Text;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.EF.Repositories;
namespace HelpDeskTickets.EF
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

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
