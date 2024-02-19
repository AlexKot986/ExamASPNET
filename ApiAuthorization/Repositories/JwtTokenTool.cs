using ApiAuthorization.AuthorizationModels.Response;
using ApiAuthorization.rsa;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiAuthorization.Repositories
{
    public class JwtTokenTool : IJwtTokenTool
    {
        public IConfiguration _config;

        public JwtTokenTool(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserResponse user)
        {
            var key = new RsaSecurityKey(RsaTools.GetPrivateKey());
            var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);

            var claim = new[]
            {
                new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
