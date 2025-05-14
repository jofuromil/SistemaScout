using BackendScout.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendScout.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;

        public JwtService(IConfiguration config)
        {
            _jwtKey = config["JwtKey"];
        }

        public string GenerarToken(User user)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("role", user.Tipo), // 🔁 Esta línea es la clave
        new Claim("Rama", user.Rama ?? "")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddHours(4),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

    }
}
