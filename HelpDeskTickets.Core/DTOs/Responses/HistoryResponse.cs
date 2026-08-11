using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.DTOs.Responses
{
    public class HistoryResponse
    {

        public int Id { get; set; }

        public int TicketId { get; set; }

        public Action action { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
