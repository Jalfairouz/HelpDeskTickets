using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskTickets.Controllers



{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHistoryService _HistoryService;

        public HistoryController(IHistoryService HistoryService, UserManager<User> userManager)
        {
            _HistoryService = HistoryService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "ITManager,Admin")]
        public async Task<IActionResult> GetTicketHistory(int ticketId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            
            var roles = await _userManager.GetRolesAsync(currentUser);
            var userRole = roles.FirstOrDefault() ?? string.Empty;
            int? departmentId = currentUser.DepartmentId; 
            var History = await _HistoryService.GetHistoryByTicketAsync(
                ticketId,
                currentUser.Id,
                userRole,
                departmentId
            );

            return Ok(History);

        }
    }
}
