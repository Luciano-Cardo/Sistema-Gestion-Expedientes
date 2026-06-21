using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Usuarios;

public class LoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ITokenService _tokenService;
    private readonly IHashService _hashService;

    public LoginUseCase(IUsuarioRepository usuarioRepo, ITokenService tokenService, IHashService hashService)
    {
        _usuarioRepo = usuarioRepo;
        _tokenService = tokenService;
        _hashService = hashService;
    }

    public LoginUsuarioResponse Ejecutar(LoginUsuarioRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CorreoElectronico) || string.IsNullOrWhiteSpace(request.contrasena))
        {
            throw new DominioException("Debe ingresar el correo y la contraseña.");
        }

        var usuario = _usuarioRepo.obtenerPorCorreo(request.CorreoElectronico);
        if (usuario == null)
        {
            throw new DominioException("El corre electronico o la contraseña son incorrectos.");
        }

        string hashEntrada = _hashService.calcularHash(request.contrasena);
        if(usuario.ContrasenaHash != hashEntrada)
        {
            throw new DominioException("El corre electronico o la contraseña son incorrectos.");
        }

        string token = _tokenService.GenerarToken(usuario.Id);
        return new LoginUsuarioResponse(token);
    }
}