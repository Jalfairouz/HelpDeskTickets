using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.Services
{
    public interface ICommentService
    {
        Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request);
        Task<IEnumerable<CommentResponse>> GetCommentsByTicketIdAsync(int ticketId);
    }
}
