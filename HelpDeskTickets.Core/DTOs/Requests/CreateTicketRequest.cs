using System.ComponentModel.DataAnnotations;
using HelpDeskTickets.Core.Models;
namespace HelpDeskTickets.DTOs.Requests
{
    public class CreateTicketRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TicketPriority Priority { get; set; }
        [Required]
        public int DepartmentId { get; set; }


    }
}
