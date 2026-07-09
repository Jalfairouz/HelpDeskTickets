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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request)
        {
            var comment = _mapper.Map<Comment>(request);

            comment.Date = DateTime.UtcNow;

            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentResponse>(comment);
        }
        public async Task<IEnumerable<CommentResponse>> GetCommentsByTicketIdAsync(int ticketId)
        {   
            var checkTicket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (checkTicket == null) return null;

            var comments = await _unitOfWork.Comments.FindAsync(c => c.TicketId == ticketId);
            return _mapper.Map<IEnumerable<CommentResponse>>(comments);
        }
    }
}
