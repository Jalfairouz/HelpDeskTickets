using HelpDeskTickets.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> AuthenticateAsync(string email, string password);
        Task<UserResponse> RegisterAsync(string email, string firstName, string lastName, string password, string role = "Customer");

    }
}
