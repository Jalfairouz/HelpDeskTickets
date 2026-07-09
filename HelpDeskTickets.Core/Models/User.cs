using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public int? DepartmentId { get; private set; }
        public Department Department { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private User() { }

        public static User Create(
           string email,
           string firstName,
           string lastName,
           string passwordHash,
           string role = "Customer",
           int? departmentId = null)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = email.ToLowerInvariant(),
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = passwordHash,
                Role = role,
                DepartmentId = departmentId,
                CreatedAt = DateTime.UtcNow
            };

        }
    }
}
