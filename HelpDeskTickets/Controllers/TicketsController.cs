using HelpDeskTickets.Core;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//In this controllor we will use Create, GetById, GetAll, Update, Delete, Change Ticket status.

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Tickets.GetAll());
        }

        [HttpPost("Create")]
        public IActionResult Add([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest();
            }
            var createdTicket = _unitOfWork.Tickets.Add(ticket);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTicket.Id }, createdTicket);
        }
    }
}
