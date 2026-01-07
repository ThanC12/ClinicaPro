using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicaPro.Api.Security;

public class JwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public (string token, DateTime expiresAtUtc) CreateToken(Guid userId, string email, string role)
    {
        // Lee sección Jwt
        var jwt = _config.GetSection("Jwt");

        // 1) Valida configuración obligatoria (evita: "Value cannot be null. (Parameter 's')")
        var keyRaw = jwt["Key"];
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var expiresMinutesRaw = jwt["ExpiresMinutes"];

        if (string.IsNullOrWhiteSpace(keyRaw))
            throw new InvalidOperationException("Falta configurar Jwt:Key en appsettings.json");

        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException("Falta configurar Jwt:Issuer en appsettings.json");

        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("Falta configurar Jwt:Audience en appsettings.json");

        if (!int.TryParse(expiresMinutesRaw, out var expiresMinutes) || expiresMinutes <= 0)
            expiresMinutes = 60; // valor por defecto

        // 2) Construye key segura
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyRaw));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

        // 3) Claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email ?? string.Empty),
            new Claim(ClaimTypes.Role, role ?? string.Empty),
            new Claim("role", role ?? string.Empty),
            new Claim("uid", userId.ToString())
        };

        // 4) Token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}
