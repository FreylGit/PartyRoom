using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PartyRoom.Domain.Entities;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PartyRoom.WebAPI.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GetToken(ApplicationUser user, IEnumerable<Claim> pronicpal)
        {
            var claims = pronicpal.ToList();

            claims.Add(new Claim("Username", user.UserName));
            claims.Add(new Claim("Id", user.Id.ToString()));
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var jwt = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
               notBefore: DateTime.UtcNow,
               signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        public Guid GetUserIdByToken(HttpContext context)
        {
            var jwtToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var userId = new Guid(token.Claims.First(claim => claim.Type == "Id").Value);
            return userId;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(65)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }
    }
}
