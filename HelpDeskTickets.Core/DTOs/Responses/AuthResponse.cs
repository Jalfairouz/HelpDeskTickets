using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.DTOs.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        
        public int ExpiresIn { get; set; }
    }
}
