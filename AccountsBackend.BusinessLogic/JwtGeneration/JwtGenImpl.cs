using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AccountsBackend.Data.Models;
using System.Security.Cryptography;

namespace AccountsBackend.BusinessLogic.JwtGeneration
{
    public class JwtGenImpl : IJwtGen
    {
        private readonly IConfiguration _configuration;
        
        public JwtGenImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenAccessToken(User user)
        {
            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.Login),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                }),
                Expires = tokenExpiryTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(key)), 
                        SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(securityToken);
        }

        public string GetRefreshToken()
        {
            var random = new byte[32];
            using (var gen = new RNGCryptoServiceProvider())
            {
                gen.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}

