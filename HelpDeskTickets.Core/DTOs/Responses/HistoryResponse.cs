using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HelpDeskTickets.Core.DTOs.Responses
{
    public class HistoryResponse
    {

        public int Id { get; set; }

        public int TicketId { get; set; }
        public string action { get; set; }

        public string PerformedByUserId { get; set; }

        public DateTime LoggedAt { get; set; }


    }
}
