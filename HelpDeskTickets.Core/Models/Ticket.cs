using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{

    public enum TicketStatus
    {
        Open,
        InProgress,
        Closed
    }
    public enum TicketPriority
    {
        Low,
        Medium,
        High
    }
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;




        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
