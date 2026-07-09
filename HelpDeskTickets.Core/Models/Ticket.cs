using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{
    //I used enums to represent the status and priority of a ticket
    //I used enums to represent the status and priority of a ticket
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
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //relation between Ticket and Comment is one-to-many
        //and DepartmentId is a foreign key in Ticket that references the primary key of Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
