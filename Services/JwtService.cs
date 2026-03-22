using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SWD392_MVC.Models;

namespace SWD392_MVC.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        // 1. Define the claims (data stored inside the token)
        var claims = new[]
        {
            new Claim("UserId",   user.UserId.ToString()),
            new Claim("UserName", user.FullName),
            new Claim("UserRole", user.Role.RoleName),
            new Claim("Email",    user.Email),
            // Standard JWT claims
            new Claim(JwtRegisteredClaimNames.Sub,  user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role,               user.Role.RoleName)
        };

        // 2. Create signing key from secret
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(
                        double.Parse(_config["Jwt:ExpireMinutes"] ?? "60"));

        // 3. Build the token
        var token = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: creds
        );

        // 4. Serialize to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var key       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var handler   = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,      // checks expiry
                ValidateIssuerSigningKey = true,
                ValidIssuer              = _config["Jwt:Issuer"],
                ValidAudience            = _config["Jwt:Audience"],
                IssuerSigningKey         = key,
                ClockSkew                = TimeSpan.Zero  // no tolerance on expiry
            };

            // Returns ClaimsPrincipal if valid, throws if invalid/expired
            return handler.ValidateToken(token, parameters, out _);
        }
        catch
        {
            return null; // invalid or expired token
        }
    }
}
