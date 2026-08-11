using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HelpDeskTickets.Core.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;

    }
}
