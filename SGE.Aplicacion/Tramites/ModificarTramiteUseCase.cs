using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IExpedienteRepository _repoExpediente;
    private readonly IAutorizacionService _autorizacion;
    private readonly ActualizacionEstadoExpedienteService _servicioEstado;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public ModificarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion, ActualizacionEstadoExpedienteService servicioEstado, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
        _servicioEstado = servicioEstado;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarTramiteResponse Ejecutar (ModificarTramiteRequest request)
    {
        if(!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion)) 
            throw new AutorizacionException("El usuario no posee autorizacion para modificar el tramite");

        var tramite = _repoTramite.ObtenerPorId(request.IdTramite);
        if(tramite == null) 
            throw new EntidadNoEncontradaException("No existe un tramite con ese ID");

        tramite.ModificarContenido(request.NuevoContenido, request.IdUsuario);
 
        _repoTramite.Modificar(tramite);

        _servicioEstado.Ejecutar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.GuardarCambios();
        return new ModificarTramiteResponse(tramite.Id, tramite.UsuarioUltimoCambio, tramite.FechaUltimaModificacion);
    }
}
