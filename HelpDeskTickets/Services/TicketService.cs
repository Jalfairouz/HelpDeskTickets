using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace HelpDeskTickets.App.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<TicketResponse?> CreateTicketAsync(CreateTicketRequest request)
        {
            var departmentCheck = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (departmentCheck == null)
            {
                return null;
            }
            var ticket = _mapper.Map<Ticket>(request);
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = TicketStatus.Open;
            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse?> GetTicketByIdAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetQueryable()
                .Include(t => t.Department)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return null;

            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<IEnumerable<TicketResponse>> GetAllTicketsAsync()
        {
            var tickets = await _unitOfWork.Tickets.GetQueryable()                    
                .Include(t => t.Department).ToListAsync();
            return _mapper.Map<IEnumerable<TicketResponse>>(tickets);
        }
        public async Task<TicketResponse?> UpdateTicketAsync(int id, UpdateTicketRequest request)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null) return null;

            _mapper.Map(request, ticket);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse?> ChangeTicketStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<TicketStatus>(status, true, out var ticketStatus))
            {
                throw new ArgumentException($"Invalid status: {status}. Valid values are: Open, InProgress, Closed");
            }

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null) return null;

            ticket.Status = ticketStatus;
            _unitOfWork.Tickets.Update(ticket);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                return false;
            }
            _unitOfWork.Tickets.Delete(ticket);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
