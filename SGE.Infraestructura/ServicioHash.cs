namespace SGE.Infraestructura;

using SGE.Aplicacion.Servicios;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

public record class ServicioHash : IHashService
{
    public String calcularHash(String contrasenaPlana)
    {
        SHA256 sha256 = SHA256.Create();
        
        byte [] vectorEntrada = Encoding.UTF32.GetBytes(contrasenaPlana);

        byte [] vectorHash = sha256.ComputeHash(vectorEntrada);

        StringBuilder constructorString = new StringBuilder();

        foreach(byte b in vectorHash)
        {
            constructorString.Append(b.ToString("x2"));
        }

        return constructorString.ToString();
    }
}