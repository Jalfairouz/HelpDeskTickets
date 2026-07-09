using System;
using System.Collections.Generic;
using System.Text;
using HelpDeskTickets.Core.Models;
using Microsoft.EntityFrameworkCore;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.EF.Data;
namespace HelpDeskTickets.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
