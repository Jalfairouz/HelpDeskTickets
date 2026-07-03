using System.ComponentModel.DataAnnotations;

namespace HelpDeskTickets.DTOs.Requests
{
    public class CreateDepartmentRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
