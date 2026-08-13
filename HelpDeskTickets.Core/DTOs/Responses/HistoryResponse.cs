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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Action action { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
