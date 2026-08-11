using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.Core.DTOs.Requests
{
    public class SetUserRoleRequest
    {
        public string Email {  get; set; }
        public string Role { get; set; } = "User";
    }
}
