using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGE.Aplicacion.Servicios;

namespace SGE.Infraestructura;

public class ServicioToken : ITokenService
{
    private readonly IConfiguration _configuration;

    public ServicioToken(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public String GenerarToken(Guid usuarioId)
    {
        string clave = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("No se encontró la clave JWT (Jwt:Key) en la configuración.");
        string issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("No se encontró el issuer JWT (Jwt:Issuer) en la configuración.");
        string audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("No se encontró el audience JWT (Jwt:Audience) en la configuración.");

        var claves = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
        var credenciales = new SigningCredentials(claves, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}