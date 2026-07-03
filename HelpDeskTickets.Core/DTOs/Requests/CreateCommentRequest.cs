using System.ComponentModel.DataAnnotations;

namespace HelpDeskTickets.DTOs.Requests
{
    public class CreateCommentRequest

    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int TicketId { get; set; }
    }
}
