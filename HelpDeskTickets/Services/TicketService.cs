using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace HelpDeskTickets.App.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public TicketService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
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

            await AutoAssignTicketAsync(ticket.Id);
            return _mapper.Map<TicketResponse>(ticket);
        }
        public async Task<TicketResponse?> GetTicketByIdAsync(
           int id,
           string userId,
           string userRole,
           int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetQueryable()
                .Include(t => t.CreatedByUser.Department)
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
                .Include(t => t.CreatedByUser.Department);

            if (userRole == "Admin")
            {
            }
            else if (userRole == "ITManager")
            {
                if (userDepartmentId == null)
                    throw new UnauthorizedAccessException("Manager must be assigned to a department");

                query = query.Where(t => t.CreatedByUser.DepartmentId == userDepartmentId);
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
                throw new ArgumentException($"Invalid status: {status}");

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);

            if (ticket == null)
                return null;

            if (userRole == "Technician" && ticket.AssignedToUserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You can only change the status of tickets assigned to you.");
            }

            ticket.Status = ticketStatus;
            ticket.UpdateAt = DateTime.UtcNow;

            if (userRole == "Technician" &&
                ticketStatus == TicketStatus.Closed)
            {
                ticket.Historys.Add(new History
                {
                    TicketId = ticket.Id,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    action = HelpDeskTickets.Core.Models.Action.Closed
                });
            }

            else if (userRole == "Technician" &&
                     ticketStatus == TicketStatus.Reject)
            {
                ticket.Historys.Add(new History
                {
                    TicketId = ticket.Id,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    action = HelpDeskTickets.Core.Models.Action.rejected
                });
            }

            _unitOfWork.Tickets.Update(ticket);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketResponse>(ticket);
        }

        //ValidateTicketAccess(ticket, userId, userRole, userDepartmentId, "update");

       
        
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
        public async Task<bool> AutoAssignTicketAsync(int ticketId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null || ticket.AssignedToUserId == null) return false;

            var allTechnician = await _userManager.GetUsersInRoleAsync("Technician");
            var AvailableTechnician = allTechnician
                .Where(t => t.IsAvailable == true)
                .ToList();
            if (!AvailableTechnician.Any()) return false;

            var selectedTechnician = AvailableTechnician.OrderBy(t => t.AssignedTickets.Count(tr => tr.AssignedToUserId != null)).FirstOrDefault();
            if (selectedTechnician == null) return false;

            ticket.AssignedToUserId = selectedTechnician.Id;
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        
        
        public async Task<TicketResponse?> AssignTicketToTechnicianAsync( int ticketId, string technicianId, string ManagerId, string userRole)
         {

            if (userRole != "ITManager")
                throw new UnauthorizedAccessException("Only ITManager can Assign tickets");
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket not found");
            if (ticket.AssignedToUserId != null) throw new Exception("Ticket was already assigned to a technician");

            var technician = await _userManager.FindByIdAsync(technicianId);
            if (technician == null || !await _userManager.IsInRoleAsync(technician, "Technician"))
                throw new Exception("The specified user is not a valid technician.");

            ticket.AssignedToUserId = technicianId;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TicketResponse>(ticket);
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
                if (ticket.CreatedByUser.DepartmentId != userDepartmentId)
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
