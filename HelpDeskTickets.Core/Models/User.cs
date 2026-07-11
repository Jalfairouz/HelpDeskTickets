using Microsoft.AspNetCore.Identity;
using HelpDeskTickets.Core.Models;

namespace HelpDeskTickets.Core.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

