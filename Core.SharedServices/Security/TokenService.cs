using Core.Contracts.Model;
using Core.SharedServices.Exceptions;
using Core.SharedServices.Security.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.SharedServices.Security
{
    public class TokenService<TUser> : ITokenService<TUser> where TUser : BaseUser
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        private readonly JwtSecurityTokenHandler _tokenHandler;


        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string CreateToken(TUser user)
        {
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);

            return _tokenHandler.WriteToken(token);
        }

        public Guid? GetUserIdFromToken(string token)
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token.Replace("Bearer ", ""));
            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (subClaim != null && Guid.TryParse(subClaim.Value, out var userId))
                return userId;

            throw new BusinessException("Token inválido",
                Contracts.Exceptions.EnumBusinessErrorCode.InvalidToken,
                "Invalid token");
        }
    }
}