using HelpDeskTickets.Core.Models;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, string role);
    }
}