using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Aplicacion.Tramites;

public class EliminarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IExpedienteRepository _repoExpediente;
    private readonly IAutorizacionService _autorizacion;
    private readonly ActualizacionEstadoExpedienteService _servicioEstado;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public EliminarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion, ActualizacionEstadoExpedienteService servicioEstado, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
        _servicioEstado = servicioEstado;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarTramiteResponse Ejecutar (EliminarTramiteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja)) 
            throw new AutorizacionException("El usuario no posee autorizacion para eliminar tramites");

        var tramite = _repoTramite.ObtenerPorId(request.IdTramite);
        if (tramite == null) 
            throw new EntidadNoEncontradaException("No existe un tramite con ese ID");
        
        _repoTramite.Eliminar(tramite.Id);
        
        _servicioEstado.Ejecutar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
        return new EliminarTramiteResponse(tramite.Id);
    }
}
