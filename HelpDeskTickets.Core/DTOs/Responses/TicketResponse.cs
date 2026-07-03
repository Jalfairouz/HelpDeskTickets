using HelpDeskTickets.Core.Models;
namespace HelpDeskTickets.DTOs.Responses
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentResponse Department { get; set; }
        public ICollection<CommentResponse> Comments { get; set; }
            = new List<CommentResponse>();
    }
}
