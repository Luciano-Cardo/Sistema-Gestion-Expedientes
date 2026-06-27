namespace SGE.Aplicacion.Servicios;

public interface ITokenService
{
    String GenerarToken(Guid UsuarioId);
}