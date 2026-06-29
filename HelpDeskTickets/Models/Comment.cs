namespace HelpDeskTickets.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
