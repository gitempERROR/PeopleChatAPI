using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PeopleChatAPI.Configuration;
using PeopleChatAPI.Dto;
using PeopleChatAPI.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PeopleChatAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly AuthOptions _authOptions;

        public JwtService(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value ?? throw new ArgumentNullException(nameof(authOptions));
        }

        public string GenerateToken(AuthDto? auth)
        {
            var claims = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, auth!.UserLogin) 
                ] 
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _authOptions.Issuer,
                Audience = _authOptions.Audience,
                Subject = claims,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
                SigningCredentials = new SigningCredentials
                (
                    AuthOptions.GetSymmetricSecurityKey(_authOptions.EncryptionKey),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
