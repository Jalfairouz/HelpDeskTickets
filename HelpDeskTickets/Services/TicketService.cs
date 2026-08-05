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
        public async Task<TicketResponse> CreateTicketAsync(CreateTicketRequest request, string userId)
        {
            var departmentCheck = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId);
            if (departmentCheck == null)
                throw new Exception("Department not found");

            var ticket = _mapper.Map<Ticket>(request);
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Status = TicketStatus.Open;
            ticket.CreatedByUserId = userId;

            await _unitOfWork.Tickets.AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse?> GetTicketByIdAsync(
           int id,
           string userId,
           string userRole,
           int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetQueryable()
                .Include(t => t.Department)
                .Include(t => t.Comments)
                .ThenInclude(c=> c.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return null;

            ValidateTicketAccess(ticket, userId, userRole, userDepartmentId, "view");

            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<IEnumerable<TicketResponse>> GetAllTicketsAsync(
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            IQueryable<Ticket> query = _unitOfWork.Tickets.GetQueryable()
                .Include(t => t.Department);

            if (userRole == "Admin")
            {
            }
            else if (userRole == "Manager")
            {
                if (userDepartmentId == null)
                    throw new UnauthorizedAccessException("Manager must be assigned to a department");

                query = query.Where(t => t.DepartmentId == userDepartmentId);
            }
            else
            {
                query = query.Where(t => t.CreatedByUserId == userId);
            }

            var tickets = await query.ToListAsync();
            return _mapper.Map<IEnumerable<TicketResponse>>(tickets);
        }
        public async Task<TicketResponse?> UpdateTicketAsync(
            int id,
            UpdateTicketRequest request,
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
                return null;

            ValidateTicketAccess(ticket, userId, userRole, userDepartmentId, "update");

            _mapper.Map(request, ticket);
            _unitOfWork.Tickets.Update(ticket);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketResponse>(ticket);
        }

        public async Task<TicketResponse?> ChangeTicketStatusAsync(
            int id,
            string status,
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            if (!Enum.TryParse<TicketStatus>(status, true, out var ticketStatus))
                throw new ArgumentException($"Invalid status: {status}. Valid values are: Open, InProgress, Closed");

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
                return null;

            ValidateTicketAccess(ticket, userId, userRole, userDepartmentId, "update");

            ticket.Status = ticketStatus;
            _unitOfWork.Tickets.Update(ticket);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<bool> DeleteTicketAsync(int id, string userRole)
        {
            if (userRole != "Admin")
                throw new UnauthorizedAccessException("Only Admin can delete tickets");

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
                return false;

            _unitOfWork.Tickets.Delete(ticket);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private void ValidateTicketAccess(
            Ticket ticket,
            string userId,
            string userRole,
            int? userDepartmentId,
            string action)
        {
            if (userRole == "Admin")
                return;

            if (userRole == "Manager")
            {
                if (ticket.DepartmentId != userDepartmentId)
                    throw new UnauthorizedAccessException(
                        $"You can only {action} tickets from your department");
                return;
            }

            if (action == "view")
            {
                if (ticket.CreatedByUserId != userId)
                    throw new UnauthorizedAccessException(
                        "You can only view your own tickets");
            }
            else if (action == "update")
            {
                throw new UnauthorizedAccessException(
                    "Users cannot modify tickets");
            }

        }
    }
}
