using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfileDto>> GetAllUsersAsync();
    }
}
