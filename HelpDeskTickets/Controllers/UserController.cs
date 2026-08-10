using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.DTOs.Responses;
using HelpDeskTickets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUsers()
        {

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
