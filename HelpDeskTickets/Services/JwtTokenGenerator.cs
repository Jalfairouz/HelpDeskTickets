using HelpDeskTickets.Core.Interfaces;
using HelpDeskTickets.Core.Models;
using HelpDeskTickets.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace HelpDeskTickets.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public JwtTokenGenerator(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
            ValidateSettings();
        }
        public string GenerateToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var securityKey = new SymmetricSecurityKey(secretKeyBytes);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Role),
                new Claim("DepartmentId", user.DepartmentId?.ToString() ?? "0")
            };

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.SecretKey))
                throw new InvalidOperationException("JwtSettings.SecretKey is not configured");

            if (_jwtSettings.SecretKey.Length < 32)
                throw new InvalidOperationException("JwtSettings.SecretKey must be at least 32 characters");

            if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer))
                throw new InvalidOperationException("JwtSettings.Issuer is not configured");

            if (string.IsNullOrWhiteSpace(_jwtSettings.Audience))
                throw new InvalidOperationException("JwtSettings.Audience is not configured");

            if (_jwtSettings.ExpirationMinutes <= 0)
                throw new InvalidOperationException("JwtSettings.ExpirationMinutes must be greater than 0");
        }

    }
}
