using SGE.Dominio.Comun;
using SGE.Dominio.Entidades;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _repoUsuario;
    private readonly IHashService _hashService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public RegistrarUsuarioUseCase(IUsuarioRepository repoUsuario, IHashService hashService, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repoUsuario = repoUsuario;
        _hashService = hashService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        if (_repoUsuario.obtenerPorCorreo(request.CorreoElectronico) != null)
        {
            throw new DominioException("Ya existe un usuario con este correo");
        }

        String contrasenaHasheada = _hashService.calcularHash(request.contrasena);
        Usuario usuario = new Usuario(request.Nombre, request.CorreoElectronico, contrasenaHasheada);

        _repoUsuario.Agregar(usuario);
        _unidadDeTrabajo.GuardarCambios();

        return new RegistrarUsuarioResponse(usuario.Id);
    }
}