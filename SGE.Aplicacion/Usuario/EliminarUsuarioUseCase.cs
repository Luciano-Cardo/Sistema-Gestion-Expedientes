using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public class EliminarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarUsuarioUseCase(IUsuarioRepository usuarioRepo, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepo = usuarioRepo;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public void Ejecutar(EliminarUsuarioRequest request)
    {
        var usuarioOrigen = _usuarioRepo.obtenerPorId(request.IdOrigen);
        if(usuarioOrigen == null || !usuarioOrigen.esAdministrador)
        {
            throw new AutorizacionException("acceso denegado: el usuario no es administrador o no existe");   
        }

        var eliminar = _usuarioRepo.obtenerPorId(request.IdAEliminar);
        if(eliminar == null)
        {
            throw new EntidadNoEncontradaException("el usuario a eliminar no existe");   
        }

        _usuarioRepo.Eliminar(eliminar);
        _unidadDeTrabajo.GuardarCambios();
    }
}