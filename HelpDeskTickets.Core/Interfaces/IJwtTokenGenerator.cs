using HelpDeskTickets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);

    }
}
