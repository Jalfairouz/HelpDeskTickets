using HelpDeskTickets.Core.DTOs.Requests;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Register request for email: {Email}", request.Email);

            var result = await _authService.RegisterAsync(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password,
                request.Role);

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            return CreatedAtAction(nameof(GetProfile), new { userId = result.Id }, result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login request for email: {Email}", request.Email);

            var result = await _authService.AuthenticateAsync(request.Email, request.Password);

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var lastName = User.FindFirst(ClaimTypes.FamilyName)?.Value;
            var role = User.FindFirst("Role")?.Value;

            var profile = new UserProfileDto 
            {
                UserId = Guid.Parse(userId),
                Email = email ?? string.Empty,
                FirstName = firstName ?? string.Empty,
                LastName = lastName ?? string.Empty,
                Role = role ?? string.Empty
            };

            return Ok(profile);
        }

    }

}
