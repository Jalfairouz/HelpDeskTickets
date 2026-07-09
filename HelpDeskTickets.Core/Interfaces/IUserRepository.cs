using HelpDeskTickets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<int> SaveChangesAsync();

    }
}
