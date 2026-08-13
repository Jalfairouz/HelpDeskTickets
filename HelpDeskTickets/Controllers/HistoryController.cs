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
        [Authorize(Roles = "Admin,ITManager")]
        public async Task<IActionResult> GetTicketHistory(int ticketId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var History = await _HistoryService.GetHistoryByTicketAsync( ticketId);

            return Ok(History);

        }
    }
}
