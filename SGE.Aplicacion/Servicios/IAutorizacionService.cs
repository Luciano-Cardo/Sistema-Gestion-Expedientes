namespace SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Autorizacion;
public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}