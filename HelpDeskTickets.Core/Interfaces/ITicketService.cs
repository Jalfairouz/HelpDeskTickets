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
        Task<TicketResponse?> CreateTicketAsync(CreateTicketRequest request, string userId);
        Task<TicketResponse?> GetTicketByIdAsync(int id, string userId, string userRole, int? userDepartmentId);
        Task<IEnumerable<TicketResponse>> GetAllTicketsAsync(string userId, string userRole, int? userDepartmentId);
        Task<TicketResponse?> UpdateTicketAsync(int id, UpdateTicketRequest request, string userId, string userRole, int? userDepartmentId);
        Task<TicketResponse?> ChangeTicketStatusAsync(int id, string status, string userId, string userRole, int? userDepartmentId);
        Task<bool> DeleteTicketAsync(int id, string userRole);

        Task<bool> AutoAssignTicketAsync(int ticketId);
        Task<TicketResponse?> AssignTicketToTechnicianAsync(int ticketId, string technicianId, string ManagerId);


    }
}
