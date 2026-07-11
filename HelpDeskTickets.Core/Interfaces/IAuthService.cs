using HelpDeskTickets.Core.DTOs.Responses;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> RegisterAsync(string email, string firstName, string lastName, string password, string role = "Customer");
        Task<AuthResponse> AuthenticateAsync(string email, string password);
    }
}