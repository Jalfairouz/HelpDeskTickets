using HelpDeskTickets.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HelpDeskTickets.DTOs.Requests;
using HelpDeskTickets.App.Services;
//Here we will use Create and Get Department Controller.

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentSrevice _departmentService;

        public DepartmentsController(IDepartmentSrevice departmentSrevice)
        {
            _departmentService = departmentSrevice;
           
        }
        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> CreateDepartmentAsync([FromBody] CreateDepartmentRequest request)
        {
            var createdDepartment = await _departmentService.CreateDepartmentAsync(request);
            return Ok(createdDepartment);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponse>>> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

    }
}
