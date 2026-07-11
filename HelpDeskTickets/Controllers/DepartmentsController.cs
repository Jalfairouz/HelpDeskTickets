using HelpDeskTickets.App.Services;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

//Here we will use Create and Get Department Controller.

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentSrevice _departmentService;

        public DepartmentsController(IDepartmentSrevice departmentSrevice)
        {
            _departmentService = departmentSrevice;
           
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DepartmentResponse>> CreateDepartment(
            [FromBody] CreateDepartmentRequest request)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var result = await _departmentService.CreateDepartmentAsync(request, userRole);
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAllDepartments()
        {

            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

    }
}
