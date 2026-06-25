using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Entidades;
namespace SGE.Aplicacion.Autorizacion;
public class AutorizacionService : IAutorizacionService
{
    private readonly IUsuarioRepository _usuarioRepo;

    public AutorizacionService(IUsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    public bool PoseeElPermiso(Guid idUsuario, Permiso permisoRequerido)
    {
        var usuario = _usuarioRepo.ObtenerPorId(idUsuario);
        
        if (usuario == null) return false;

        if(usuario.EsAdministrador) return true;

        if(permisoRequerido == Permiso.TramiteBaja && usuario.ListaPermisos.Contains(Permiso.ExpedienteBaja))
        {
            return true;
        }

        return usuario.ListaPermisos.Contains(permisoRequerido);
    } 
}