using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Usuarios;

public class ModificarPermisosUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarPermisosUsuarioUseCase(IUsuarioRepository repoUsuario, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepo = repoUsuario;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public void Ejecutar(ModificarPermisosUsuarioRequest request)
    {
        var usuarioOrigen = _usuarioRepo.obtenerPorId(request.IdOrigen);
        if(usuarioOrigen == null || !usuarioOrigen.esAdministrador)
        {
            throw new AutorizacionException("acceso denegado: el usuario no es administrador o no existe");   
        }

        var usuarioAEditar = _usuarioRepo.obtenerPorId(request.IdAEditar);
        if(usuarioAEditar == null)        {
            throw new EntidadNoEncontradaException("el usuario a editar no existe");
        }

        if (request.asignar)
        {
            usuarioAEditar.AsignarPermiso(request.nuevoPermiso);
        }
        else
        {
            usuarioAEditar.RemoverPermiso(request.nuevoPermiso);
        }

        _unidadDeTrabajo.GuardarCambios();
    }
}