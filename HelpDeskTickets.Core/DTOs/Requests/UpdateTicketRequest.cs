namespace HelpDeskTickets.DTOs.Requests
{
    public class UpdateTicketRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        

    }
}
