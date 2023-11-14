using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
            => _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        [HttpPost]
        public IActionResult Login([FromBody] Credential credential)
        {
            if (credential.Username != "Admin" && credential.Password != "123")
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized to access this endpoint");
                return Unauthorized(ModelState);
            }

            List<Claim> claims = new()
            {
                new Claim("Admin", "true"),
                new Claim("ProbationDate", "2023-07-01"),
                new Claim("UserName", credential.Username),
            };

            DateTime expiryTime = DateTime.UtcNow.AddMinutes(10);

            return Ok(new
            {
                accessToken = GetAccessToken(claims, expiryTime),
                expiryTime,
            });
        }

        private string GetAccessToken(IEnumerable<Claim> listClaims, DateTime expiryTime) 
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretToken") ?? "");

            JwtSecurityToken jwtToken = new (
                claims: listClaims, 
                notBefore: DateTime.UtcNow, 
                expires: expiryTime, 
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
