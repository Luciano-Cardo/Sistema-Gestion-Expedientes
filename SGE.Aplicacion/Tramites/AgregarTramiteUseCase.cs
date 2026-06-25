using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IExpedienteRepository _repoExpediente;
    private readonly IAutorizacionService _autorizacion;
    private readonly ActualizacionEstadoExpedienteService _servicioEstado;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public AgregarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion, ActualizacionEstadoExpedienteService servicioEstado, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
        _servicioEstado = servicioEstado;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        if(!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.TramiteAlta)) 
            throw new AutorizacionException("El usuario no posee autorizacion para agregar tramites");

        var expediente = _repoExpediente.ObtenerPorId(request.ExpedienteId);
        if(expediente == null) 
            throw new EntidadNoEncontradaException("No existe un expediente con ese ID");

        Tramite nuevoTramite = new Tramite(request.ExpedienteId, request.Etiqueta, request.Contenido, request.UsuarioUltimoCambio);

        _repoTramite.Agregar(nuevoTramite);

        _servicioEstado.Ejecutar(request.ExpedienteId, request.UsuarioUltimoCambio);
        _unidadDeTrabajo.Guardar();
        return new AgregarTramiteResponse(nuevoTramite.Id, nuevoTramite.FechaCreacion);
    } 
}