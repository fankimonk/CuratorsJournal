using Application.Entities;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Application.Authorization
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public AuthToken GenerateToken(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken(user);

            return new AuthToken { AccessToken = accessToken, RefreshToken = refreshToken };
        }
        
        private RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMonths(1),
                Enabled = true,
                UserId = user.Id
            };

            return refreshToken;
        }

        private string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(CustomClaims.UserId, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.Role!.Name)
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_options.ExpiresHours),
                SigningCredentials = signingCredentials,
                Issuer = _options.Issuer,
                Audience = _options.Audience
            };

            //Old
            //var token = new JwtSecurityToken(
            //    claims: claims,
            //    signingCredentials: signingCredentials,
            //    expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            //var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            //return tokenValue;

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
