using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ECommWeb.Core.src.Entity;
using ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

namespace ECommWeb.Infrastructure.src.Service;


public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetToken(User foundUser)
    {
        /*
        build a token
        - actual data to be encoded{email,firstname,lastname,profilePicture, etc ...}
        - encoded key {secret} - use this key to encrypt the token
        - jwt handler -> library to transfer data into token
   */

        //claims (data to be encoded) should be in form of List

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email,foundUser.Email),
            new Claim(ClaimTypes.NameIdentifier,foundUser.Id.ToString()),
            new Claim(ClaimTypes.Role,foundUser.Role.ToString()),
        };

        //key (secret key)
        var jwtKey = _configuration["Secrets:JwtKey"];

        if (jwtKey is null)
        {
            throw new ArgumentNullException("Jwt key is not found in appsettings.json");
        }
        var securityKey = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                            SecurityAlgorithms.HmacSha256Signature
        );

        // token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = securityKey,
            Issuer = _configuration["Secrets:Issuer"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
