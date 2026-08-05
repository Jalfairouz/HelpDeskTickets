using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }= DateTime.Now;

        
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;


    }
}
