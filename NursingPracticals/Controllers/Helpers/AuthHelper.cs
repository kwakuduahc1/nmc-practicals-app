using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduApp.Controllers.Helpers
{
    public class AuthHelper(IList<Claim> claims, IAppFeatures app)
    {
        public string Key
        {
            get
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.Key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var date = DateTime.UtcNow.AddHours(app.Hours);
                Console.WriteLine(date);
                var tokeOptions = new JwtSecurityToken(
                    issuer: app.Issuer,
                    audience: app.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(app.Hours),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return tokenString;
            }
        }
    }
}
