using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core.DTOs.Requests;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

//In this controllor we will use Create, GetById, GetAll, Update, Delete, Change Ticket status.

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpPost]
        public async Task<ActionResult<TicketResponse>> CreateTicket([FromBody] CreateTicketRequest request)
        {
            var createdTicket = await _ticketService.CreateTicketAsync(request);
            return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, createdTicket);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketResponse>> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
        {
            var updatedTicket = await _ticketService.UpdateTicketAsync(id, request);
            if (updatedTicket == null)
            {
                return NotFound();
            }
            return Ok(updatedTicket);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return NoContent();

        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeTicketStatus(int id, [FromBody] ChangeStatusRequest request)
        {
            await _ticketService.ChangeTicketStatusAsync(id, request.Status); 

            var updatedTicket = await _ticketService.GetTicketByIdAsync(id); 
            if (updatedTicket == null) return NotFound();
            return Ok(updatedTicket);
        }
    }
}
