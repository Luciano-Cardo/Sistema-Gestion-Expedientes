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
    private readonly ActualizacionEstadoExpedienteService _servicioEstado;

    public EliminarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion, ActualizacionEstadoExpedienteService servicioEstado)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
        _servicioEstado = servicioEstado;
    }

    public EliminarTramiteResponse Ejecutar (EliminarTramiteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja)) 
            throw new EntidadNoEncontradaException("El usuario no posee autorizacion para eliminar tramites");

        var tramite = _repoTramite.ObtenerPorId(request.IdTramite);
        if (tramite == null) 
            throw new EntidadNoEncontradaException("No existe un tramite con ese ID");
        
        _repoTramite.Eliminar(tramite.Id);
        
        _servicioEstado.Ejecutar(tramite.ExpedienteId, request.IdUsuario);

        return new EliminarTramiteResponse(tramite.Id);
    }
}
