using System.ComponentModel.DataAnnotations;

namespace HelpDeskTickets.Core.DTOs.Requests
{
    public class ChangeStatusRequest
    {
        [Required]
        [RegularExpression("Open|InProgress|Closed")]
        public string Status { get; set; }
    }
}
