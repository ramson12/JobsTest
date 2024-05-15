using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/v1/login")]
    public class AuthenticateController : Controller
    {
        
        private const string MockUsername = "admin";
        private const string MockPassword = "password";
        private readonly IConfiguration _configuration;


        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            // Check if the provided credentials are valid
            if (login.Username != MockUsername || login.Password != MockPassword)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var jwtSettings = _configuration.GetSection("Jwt");

            // Generate a JWT token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audiance"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                    SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return the JWT token to the client
            return Ok(new { Token = tokenString });
        }
    }
}
