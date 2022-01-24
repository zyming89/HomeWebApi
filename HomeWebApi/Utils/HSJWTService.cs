using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace HomeWebApi.Utils
{
    public class HSJWTService : IConstomJWTService
    {

        private readonly JWTTokenOptions _JWTTokenOptions;
        public HSJWTService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions)
        {
            _JWTTokenOptions = jwtTokenOptions.CurrentValue;
        }
        public string GetToken(string Code, string password)
        {

            var claims = new[]
            {
               new  Claim(ClaimTypes.Name,Code),
               new Claim("time",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
               new Claim("code",Code),       
               new  Claim("ABC","ABC"),
           };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTTokenOptions.SecurityKey));

            SigningCredentials cresd = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _JWTTokenOptions.Issuer,
                audience: _JWTTokenOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5), // 5分钟有效
                signingCredentials: cresd
                );
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}