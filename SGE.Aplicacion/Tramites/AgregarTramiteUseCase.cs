using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase
{
    ITramiteRepository _repoTramite;
    IExpedienteRepository _repoExpediente;
    IAutorizacionService _autorizacion;

    public AgregarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExpediente, IAutorizacionService autorizacion)
    {
        _repoTramite = repoTramite;
        _repoExpediente = repoExpediente;
        _autorizacion = autorizacion;
    }

    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        if(!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio,Permiso.TramiteAlta)) throw new AutorizacionException("No tiene permisos");

        var nuevoExpediente = _repoExpediente.ObtenerPorId(request.UsuarioUltimoCambio);

        if(nuevoExpediente == null) throw new AutorizacionException("No existe un Tramite con ese ID");
        Tramite nuevoTramite = new Tramite(request.ExpedienteId,request.Etiqueta,request.Contenido,request.UsuarioUltimoCambio);

        ActualizacionEstadoExpedienteService s = new ActualizacionEstadoExpedienteService (_repoExpediente,_repoTramite);
        s.Ejecutar(nuevoTramite.ExpedienteId,nuevoTramite.Id);

        return new AgregarTramiteResponse(nuevoTramite.UsuarioUltimoCambio,nuevoTramite.FechaCreacion);

    }
}
