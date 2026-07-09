using HelpDeskTickets.App.Services;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//Here we will create comment to ticket and Get Ticket Comments Controller
namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost]
        public async Task<ActionResult<CommentResponse>> CreateComment([FromBody] CreateCommentRequest request)
        {
            var createdComment = await _commentService.CreateCommentAsync(request);
            return CreatedAtAction(nameof(GetCommentsByTicketId), new { ticketId = createdComment.TicketId }, createdComment);
        }
        [HttpGet("ticket/{ticketId}")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByTicketId(int ticketId)
        {

            var comments = await _commentService.GetCommentsByTicketIdAsync(ticketId);
            if (comments == null) return BadRequest("Ticket not found.");
            return Ok(comments);
        }

    }
}
