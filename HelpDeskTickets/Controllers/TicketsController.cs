using HelpDeskTickets.App.Services;
using HelpDeskTickets.Core.DTOs.Requests;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//In this controllor we will use Create, GetById, GetAll, Update, Delete, Change Ticket status.

namespace HelpDeskTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly UserManager<User> _userManager;

        public TicketController(ITicketService ticketService, UserManager<User> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _ticketService.CreateTicketAsync(request, userId);
                return CreatedAtAction(nameof(GetTicketById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "ITManager,Admin")]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _ticketService.GetAllTicketsAsync(userId, userRole, user?.DepartmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _ticketService.GetTicketByIdAsync(id, userId, userRole, user?.DepartmentId);
                if (result == null)
                    return NotFound();

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

        [HttpPut("{id}")]
        [Authorize(Roles = "ITManager,Admin")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketRequest request)
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _ticketService.UpdateTicketAsync(id, request, userId, userRole, user?.DepartmentId);
                if (result == null)
                    return NotFound();

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var result = await _ticketService.DeleteTicketAsync(id, userRole);

                if (!result)
                    return NotFound();

                return NoContent();
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

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "ITManager,Admin")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var user = await _userManager.FindByIdAsync(userId);

                var result = await _ticketService.ChangeTicketStatusAsync(id, request.Status, userId, userRole, user?.DepartmentId);
                if (result == null)
                    return NotFound();

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