using HelpDeskTickets.Core;
using HelpDeskTickets.Core.DTOs.Responses;
using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.Settings;

namespace HelpDeskTickets.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;

        private static readonly string[] ValidRoles = { "Admin", "Manager", "Customer" };

        public AuthService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            JwtSettings jwtSettings,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<AuthResponse?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            email = email.Trim().ToLowerInvariant();

            _logger.LogInformation("Authenticating user with email: {Email}", email);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User not found with email: {Email}", email);
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password attempt for email: {Email}", email);
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            _logger.LogInformation("User authenticated successfully: {Email}", email);

            return new AuthResponse
            {
                Token = token,
                User = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    DepartmentId = user.DepartmentId
                },
                ExpiresIn = (int)expiresAt.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }

        public async Task<UserResponse> RegisterAsync(
            string email,
            string firstName,
            string lastName,
            string password,
            string role = "Customer")
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty", nameof(email));

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("FirstName cannot be null or empty", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("LastName cannot be null or empty", nameof(lastName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            if (password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters", nameof(password));

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be null or empty", nameof(role));

            if (!ValidRoles.Contains(role))
                throw new ArgumentException(
                    $"Role must be one of: {string.Join(", ", ValidRoles)}",
                    nameof(role));

            email = email.Trim().ToLowerInvariant();
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            role = role.Trim();

            _logger.LogInformation("Registering new user with email: {Email}", email);

            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                _logger.LogWarning("User already exists with email: {Email}", email);
                throw new InvalidOperationException($"User with email '{email}' already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(password);
            var user = User.Create(email, firstName, lastName, passwordHash, role);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Email}", email);

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                DepartmentId = user.DepartmentId
            };
        }
    }
}