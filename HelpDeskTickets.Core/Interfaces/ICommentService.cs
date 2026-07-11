using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.Services
{
    public interface ICommentService
    {
        Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request, string userId, string userRole, int? userDepartmentId);
        Task<IEnumerable<CommentResponse>> GetCommentsByTicketAsync(int ticketId, string userId, string userRole, int? userDepartmentId);

    }
}
