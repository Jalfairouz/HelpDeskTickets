using AutoMapper;
using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<CommentResponse> CreateCommentAsync(
            CreateCommentRequest request,
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
            if (ticket == null)
                throw new Exception($"Ticket with ID {request.TicketId} not found");

            ValidateCommentAccess(ticket, userId, userRole, userDepartmentId, "create");

            var comment = new Comment
            {
                Content = request.Content,
                TicketId = request.TicketId,
                CreatedByUserId = userId,
                Date = DateTime.UtcNow
            };

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentResponse>(comment);
        }
        public async Task<IEnumerable<CommentResponse>> GetCommentsByTicketAsync(
            int ticketId,
            string userId,
            string userRole,
            int? userDepartmentId)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new Exception($"Ticket with ID {ticketId} not found");

            ValidateCommentAccess(ticket, userId, userRole, userDepartmentId, "view");

            var comments = await _unitOfWork.Comments.FindAsync(c => c.TicketId == ticketId);

            return _mapper.Map<IEnumerable<CommentResponse>>(comments);
        }
        private void ValidateCommentAccess(
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
                        $"You can only {action} comments on tickets from your department");
                return;
            }

            if (ticket.CreatedByUserId != userId)
                throw new UnauthorizedAccessException(
                    $"You can only {action} comments on your own tickets");
        }
    }
}
