using Azure.Identity;
using KrushiSevaKendraMiniProject.Context;
using KrushiSevaKendraMiniProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KrushiSevaKendraMiniProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        DatabaseContext _context;
        IConfiguration _configuration;

        public AuthenticationController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("{login}")]
        public async Task<IActionResult> login([FromBody]Admin admin)
        {
            var admins = _context.Admins.Where(a => a.Username == admin.Username && a.Password == admin.Password).ToList();
            if(admins.Count > 0)
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", admins[0].Id.ToString()),
                new Claim("Name", admins[0].Name.ToString()),
                new Claim("Username", admins[0].Username.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddHours(24),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                admins[0].TokenString = stringToken;
                return Ok(admins);
            }
            else
            {
                return Ok(admins);
            }
            
        }
    

        [HttpPut]

        [Route("changepassword/{id}/{oldpassword}/{newpassword}")]

        public async Task<IActionResult> changepassword([FromRoute]int id, [FromRoute]string oldpassword, [FromRoute]string newpassword)
        {
            var admin = _context.Admins.FindAsync(id);
            if(admin.Result.Password == oldpassword)
            {
                admin.Result.Password = newpassword;
                await _context.SaveChangesAsync();
                return Ok(admin);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
