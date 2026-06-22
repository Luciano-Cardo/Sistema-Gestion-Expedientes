using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Entidades;
using System.Linq;

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
        var usuariosResponse = usuarios
            .Select(u => new UsuarioResumenResponse(u.Id, u.Nombre, u.CorreoElectronico, u.esAdministrador, u.listaPermisos))
            .ToList();
        return new ListarUsuariosResponse(usuariosResponse);
    }
}