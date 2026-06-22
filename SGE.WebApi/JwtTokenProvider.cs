using SGE.Aplicacion.Servicios;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;   
using System.Security.Claims;
using System.Text;
using SGE.Dominio.Entidades;
using Microsoft.Extensions.Configuration;
using System.Data;

public class JwtTokenProvider(IConfiguration config) : ITokenService
{
    public string GenerarToken(Guid usuarioId)
    {
        var claims = new [] {new Claim("ID", usuarioId.ToString())};
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jtw:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer : config ["Jwt:Issuer"],
            audience : config["Jwt:Audience"],
            claims : claims,
            expires : DateTime.UtcNow.AddHours(2),
            signingCredentials : creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token); 
    } 
}