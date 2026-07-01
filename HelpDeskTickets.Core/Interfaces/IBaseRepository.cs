using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IBaseRepository <T> where T : class
    {
        IEnumerable<T> GetAll ();
        Task<T> GetByIdAsync(int id);
        T Add(T entity);

    }
}
