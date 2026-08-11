using HelpDeskTickets.Core.DTOs.Requests;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.Settings;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace HelpDeskTickets.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<User> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSettings = jwtSettings;
        }

        public async Task<UserResponse> RegisterAsync(
            string email,
            string firstName,
            string lastName,
            string password,
            string role = "User")
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");

            var validRoles = new[] { "Admin", "ITManager","Technician", "User" };
            if (!validRoles.Contains(role))
                throw new ArgumentException($"Invalid role: {role}");

            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, role);

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                DepartmentId = user.DepartmentId
            };
        }
        public async Task<UserResponse> AssignRoleToUserAsync(string email, string roleName)
        {

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");
            var validRoles = new[] { "Admin", "ITManager", "Technician", "User" };
            if (!validRoles.Contains(roleName))
                throw new ArgumentException($"Invalid role: {roleName}");
            
            var user = await _userManager.FindByEmailAsync(email);

            await _userManager.AddToRoleAsync(user, roleName);

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roleName,
                DepartmentId = user.DepartmentId
            };

        }
        public async Task<AuthResponse> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password are required");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";

            var token = _jwtTokenGenerator.GenerateToken(user, userRole);

            return new AuthResponse
            {
                Token = token,
                
                ExpiresIn = _jwtSettings.ExpirationMinutes * 60
            };
        }
    }
}