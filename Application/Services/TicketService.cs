using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
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
        public async Task<TicketResponse> CreateTicketAsync(CreateTicketRequest request)
        {
            var ticket = _mapper.Map<Ticket>(request);
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = TicketStatus.Open;
            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse> GetTicketByIdAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null) throw new KeyNotFoundException($"Ticket with id {id} not found.");
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<IEnumerable<TicketResponse>> GetAllTicketsAsync()
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            return _mapper.Map<IEnumerable<TicketResponse>>(tickets);
        }
        public async Task<TicketResponse> UpdateTicketAsync(int id, UpdateTicketRequest request)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            _mapper.Map(request, ticket);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse> ChangeTicketStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<TicketStatus>(status, true, out var ticketStatus))
            {
                throw new ArgumentException($"Invalid status: {status}. Valid values are: Open, InProgress, Closed");
            }

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            ticket.Status = ticketStatus;
            _unitOfWork.Tickets.Update(ticket);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            _unitOfWork.Tickets.Delete(ticket);
            await _unitOfWork.SaveChangesAsync();

        }

    }
}
