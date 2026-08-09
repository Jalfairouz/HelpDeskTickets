using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


namespace HelpDeskTickets.Core.Models
{

    
    public enum TicketType
    {
        ServiceRequest,
        Incident
    }
    public enum TicketCategory
    {
        Hardware,       
        Software,       
        Network,        
        AccessControl,  
        Email,          
        Security,       
        GeneralInquiry
    }
    public enum TicketStatus
    {
        Open,
        InProgress,
        Reject,
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
        public TicketType Type { get; set; }
        public TicketCategory Category { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        public TimeSpan? SolutionTime { get; set; }


      
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;

        public string? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }

        public Feedback? Feedback { get; set; }

        //public ICollection<History> Historys { get; set; } = new List<History>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
