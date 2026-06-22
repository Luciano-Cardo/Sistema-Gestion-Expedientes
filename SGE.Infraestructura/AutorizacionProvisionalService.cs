using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Servicios;
namespace SGE.Infraestructura;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso Permiso) => true;
}