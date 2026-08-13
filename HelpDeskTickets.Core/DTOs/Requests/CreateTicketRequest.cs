using HelpDeskTickets.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace HelpDeskTickets.DTOs.Requests
{
    public class CreateTicketRequest
    {

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketType Type { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketCategory Category { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketPriority Priority { get; set; }




    }
}
