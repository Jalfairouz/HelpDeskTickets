namespace HelpDeskTickets.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
