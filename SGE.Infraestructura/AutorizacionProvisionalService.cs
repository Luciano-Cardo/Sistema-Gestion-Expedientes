using System;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Infraestructura;

public interface AutorizacionProvisionalService : IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso Permiso) => true;

}
