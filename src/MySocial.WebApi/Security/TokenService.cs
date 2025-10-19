using Microsoft.IdentityModel.Tokens;
using MySocial.Application.Contracts.Documents.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MySocial.WebApi.Security
{
    public class TokenService
    {
        public string GenerateToken(LoginResponse login)
        {
            var secretKey = Encoding.UTF8.GetBytes(TokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {

                Expires = DateTime.UtcNow.AddMinutes(TokenSettings.ExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, login.Id.ToString()),
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Email, login.Email),
                ])
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
