﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PartyRoom.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
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
    }
}