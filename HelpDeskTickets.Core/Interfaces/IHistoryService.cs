using HelpDeskTickets.Core.Models;

namespace HelpDeskTickets.App.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<History>> GetHistoryByTicketAsync(
            int ticketId,
            string userId,
            string userRole,
            int? userDepartmentId);
    }
}