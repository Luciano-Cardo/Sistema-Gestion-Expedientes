using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _repo;
    private readonly IAutorizacionService _autorizacion;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public  CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repo = repo;
        _autorizacion = autorizacion;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("El usuario no posee la autorizacion para cambiar el estado del expediente");

        var expediente = _repo.ObtenerPorId(request.Id);
        if(expediente == null) 
            throw new EntidadNoEncontradaException("No existe expediente con ese ID");

        expediente.CambiarEstado(request.NuevoEstado,request.UsuarioUltimoCambio);

        _repo.Modificar(expediente);
        _unidadDeTrabajo.Guardar();

        return new CambiarEstadoExpedienteResponse(expediente.Id, expediente.Estado, expediente.FechaUltimaModificacion);
    }
}
