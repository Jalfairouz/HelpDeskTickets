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

        //relation between Comment and Ticket is many-to-one
        //and TicketId is a foreign key in Comment
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;


    }
}
