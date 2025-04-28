using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMgnrAPI.Data;
using TaskMgnrAPI.DTOs;
using TaskMgnrAPI.Entities;

namespace TaskMgnrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            //if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            //{
            //    return Unauthorized("Invalid username or password.");
            //}
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash) )
            {
                return Unauthorized("Invalid username or password.");
            }
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                Id = user.Id,
                username = user.Username,
                role = user.Role
            });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
