using System.Security.Claims;

namespace SGE.WebApi;

public static class ClaimsPrincipalExtensions
{
    public static Guid ObtenerUserId(this ClaimsPrincipal user)
    {
        var valor = user.FindFirstValue("ID");

        if (string.IsNullOrWhiteSpace(valor) || !Guid.TryParse(valor, out var userId))
        {
            throw new UnauthorizedAccessException("El token no contiene un identificador de usuario válido.");
        }

        return userId;
    }
}
