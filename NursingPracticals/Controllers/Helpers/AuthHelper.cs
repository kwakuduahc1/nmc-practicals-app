using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NursingPracticals.Controllers.Helpers
{
    public class AuthHelper(IList<Claim> claims, IAppFeatures app)
    {
        public string GetKey()
        {
            Console.WriteLine(app.Key);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(app.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: app.Issuer,
                audience: app.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(app.Hours),
                signingCredentials: signinCredentials
            );
           return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}
