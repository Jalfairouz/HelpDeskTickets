using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Models
{


    public enum Action
    {
        AssigntBySystem,
        AssigntByItManager,
        Closed,
        rejected
    }





    public class History
    {
        public int Id { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;
        public User CreatedByUser { get; set; } = null!;
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Action action { get; set; }

    }
}
