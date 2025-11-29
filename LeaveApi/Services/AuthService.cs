using BCrypt.Net;
using LeaveApi.Dto;
using LeaveLibrary.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaveApi.Services
{
    public class AuthService
    {
        private readonly EmployeeRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(EmployeeRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // Login with hashed password verification
        public async Task<LoginResponseDto?> Login(LoginDto dto)
        {
            var emp = await _repo.GetByEmail(dto.Email);
            if (emp == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, emp.PasswordHash))
                return null;

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new Exception("JWT key missing"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim(ClaimTypes.Role, emp.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Role = emp.Role,
                EmployeeName = emp.Name
            };
        }

        // Hash password
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
