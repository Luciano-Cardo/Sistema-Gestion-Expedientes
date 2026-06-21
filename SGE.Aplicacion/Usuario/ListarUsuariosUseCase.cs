using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Entidades;

namespace SGE.Aplicacion.Usuarios;

public class ListarUsuariosUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    public ListarUsuariosUseCase(IUsuarioRepository usuariorepo)
    {
        _usuarioRepo = usuariorepo;
    }

    public ListarUsuariosResponse Ejecutar(ListarUsuariosRequest request)
    {
        var origen = _usuarioRepo.obtenerPorId(request.Id);
        if(origen == null || !origen.esAdministrador)
        {
            throw new AutorizacionException("Acceso denegado: se necesitan permisos de administrador.");
        }

        List<Usuario> usuarios = _usuarioRepo.ObtenerTodos();
        //se podria crear una nueva lista sin exponer las contrasenas(?)
        return new ListarUsuariosResponse(usuarios);
    }
}