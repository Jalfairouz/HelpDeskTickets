namespace HelpDeskTickets.DTOs.Responses
{
    public class CommentResponse
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int TicketId { get; set; }
    }
}
