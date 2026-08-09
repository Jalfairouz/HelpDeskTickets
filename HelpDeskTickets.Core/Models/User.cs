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
        public bool IsActive { get; set; } = true;
        public bool? IsAvailable { get; set; }
        public int CurrentTicketsCount { get; set; } = 0;

        public ICollection<Ticket> UserTickets { get; set; } = new List<Ticket>();
        public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

