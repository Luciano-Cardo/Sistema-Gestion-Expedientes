using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
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
        var origen = _usuarioRepo.ObtenerPorId(request.Id);
        if(origen == null || !origen.EsAdministrador)
        {
            throw new AutorizacionException("Acceso denegado: se necesitan permisos de administrador.");
        }

        List<Usuario> usuarios = _usuarioRepo.ObtenerTodos();
        return new ListarUsuariosResponse(usuarios);
    }
}