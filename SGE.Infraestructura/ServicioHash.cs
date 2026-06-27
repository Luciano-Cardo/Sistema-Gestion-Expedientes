using System.Security.Cryptography;
using System.Text;
using SGE.Aplicacion.Servicios;

namespace SGE.Infraestructura;

public class ServicioHash : IHashService 
{
    public string CalcularHash(string contrasenaPlana)
    {
        using var sha256 = SHA256.Create();
        
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasenaPlana));
        
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}