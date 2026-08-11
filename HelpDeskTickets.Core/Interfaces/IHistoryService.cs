using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Models;

namespace HelpDeskTickets.App.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryResponse>> GetHistoryByTicketAsync(
            int ticketId);
           
    }
}