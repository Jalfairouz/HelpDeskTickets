using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

//Here we will create comment to ticket and Get Ticket Comments Controller
namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentsController(ICommentService commentService,  UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<ActionResult<CommentResponse>> CreateComment(
            [FromBody] CreateCommentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _commentService.CreateCommentAsync(request, userId, userRole, user?.DepartmentId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ticket/{ticketId}")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByTicket(int ticketId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _commentService.GetCommentsByTicketAsync(ticketId, userId, userRole, user?.DepartmentId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
