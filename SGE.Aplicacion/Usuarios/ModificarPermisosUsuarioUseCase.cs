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
        var usuarioOrigen = _usuarioRepo.ObtenerPorId(request.IdOrigen);
        if(usuarioOrigen == null || !usuarioOrigen.EsAdministrador)
        {
            throw new AutorizacionException("acceso denegado: el usuario no es administrador o no existe");   
        }

        var usuarioAEditar = _usuarioRepo.ObtenerPorId(request.IdAEditar);
        if(usuarioAEditar == null)        {
            throw new EntidadNoEncontradaException("el usuario a editar no existe");
        }

        if (request.Asignar)
        {
            usuarioAEditar.AsignarPermiso(request.NuevoPermiso);
        }
        else
        {
            usuarioAEditar.RemoverPermiso(request.NuevoPermiso);
        }

        _unidadDeTrabajo.Guardar();
    }
}