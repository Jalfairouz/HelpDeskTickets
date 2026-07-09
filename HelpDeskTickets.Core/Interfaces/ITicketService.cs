using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.App.Services
{
    public interface ITicketService
    {
        Task<TicketResponse> CreateTicketAsync(CreateTicketRequest request);
        Task<TicketResponse?> GetTicketByIdAsync(int id);
        Task<IEnumerable<TicketResponse>> GetAllTicketsAsync();
        Task<TicketResponse?> UpdateTicketAsync(int id, UpdateTicketRequest request);
        Task <TicketResponse?>ChangeTicketStatusAsync(int id, string status);
        Task<bool> DeleteTicketAsync(int id);
        
    }
}
