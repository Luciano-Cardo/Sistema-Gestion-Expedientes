using System.Runtime.CompilerServices;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
namespace SGE.Aplicacion.Tramites;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IExpedienteRepository _repoExpediente;

    private readonly IAutorizacionService _autorizacion;

    public EliminarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
    }

    public EliminarTramiteResponse Ejecutar (EliminarTramiteRequest request)
    {
        if(!_autorizacion.PoseeElPermiso(request.id, Permiso.TramiteBaja)) throw new AutorizacionException("No posee el permiso");

        var tramite = _repoTramite.ObtenerPorId(request.IdTramiteEliminar);
        if(tramite == null) throw new AutorizacionException("No existe un tramite con ese ID");
        
        ActualizacionEstadoExpedienteService actualizar = new ActualizacionEstadoExpedienteService(_repoExpediente,_repoTramite);
        actualizar.Ejecutar(tramite.ExpedienteId,tramite.Id); //preguntar esto
        _repoTramite.Eliminar(tramite.Id);

        return new EliminarTramiteResponse(tramite.Id);
    }
}
