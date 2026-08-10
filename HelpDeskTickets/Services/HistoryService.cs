using AutoMapper;
using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskTickets.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<History>> GetHistoryByTicketAsync(
            int ticketId,
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
            {
                return Enumerable.Empty<History>();
            }

            bool hasAccess = userRole switch
            {
                "Admin" => true,
                "ITManager" => ticket.CreatedByUser.DepartmentId == userDepartmentId,
                _ => ticket.CreatedByUserId == userId
            };

            if (!hasAccess)
            {
                return Enumerable.Empty<History>();
            }

            var histories = await _unitOfWork.Historys.FindAsync(h => h.TicketId == ticketId);

            return histories.OrderByDescending(h => h.Ticket.UpdateAt);
        }
    }
}