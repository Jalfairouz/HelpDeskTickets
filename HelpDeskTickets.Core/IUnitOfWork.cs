using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core
{
    public interface IUnitOfWork : IDisposable    
    {
        IBaseRepository<Ticket> Tickets { get; }
        IBaseRepository<Comment> Comments { get; }
        IBaseRepository<Department> Departments { get; }

        int Complete();

    }
}
