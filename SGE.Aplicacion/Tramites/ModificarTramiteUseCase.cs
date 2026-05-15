using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IExpedienteRepository _repoExpediente;

    private readonly IAutorizacionService _autorizacion;

    public ModificarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
    }

    public ModificarTramiteResponse Ejecutar (ModificarTramiteRequest request)
    {
        if(!_autorizacion.PoseeElPermiso(request.id,Permiso.TramiteModificacion)) throw new AutorizacionException("No posee el permiso");

        var tramite = _repoTramite.ObtenerPorId(request.id);

        if(tramite == null) throw new AutorizacionException("No existe un tramite con ese id");

        tramite.ModificarContenido(request.NuevoContenidoTramite,request.id);
        ActualizacionEstadoExpedienteService actualizar = new ActualizacionEstadoExpedienteService(_repoExpediente,_repoTramite);
        actualizar.Ejecutar(tramite.ExpedienteId,tramite.Id);
        _repoTramite.Modificar(tramite);
        
        return new ModificarTramiteResponse(tramite.Id,tramite.UsuarioUltimoCambio,tramite.FechaUltimaModificacion);
    }
}
