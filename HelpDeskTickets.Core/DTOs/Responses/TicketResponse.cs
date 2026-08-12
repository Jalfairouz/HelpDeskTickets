using HelpDeskTickets.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace HelpDeskTickets.DTOs.Responses
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketType Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketCategory Category { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public string AssignedToUserName { get; set; }
        public string? AssignedToUserId { get; set; }


    }
}
