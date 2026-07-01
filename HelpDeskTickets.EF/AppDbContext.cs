using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HelpDeskTickets.Core.Models;
namespace HelpDeskTickets.EF
{
    public  class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
