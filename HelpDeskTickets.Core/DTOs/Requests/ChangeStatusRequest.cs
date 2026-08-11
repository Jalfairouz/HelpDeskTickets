using System.ComponentModel.DataAnnotations;

namespace HelpDeskTickets.Core.DTOs.Requests
{
    public class ChangeStatusRequest
    {
        [Required]
        [RegularExpression("Open|InProgress|Reject|Closed")]
        public string Status { get; set; }
    }
}
