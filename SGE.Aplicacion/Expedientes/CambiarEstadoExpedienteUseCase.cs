using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _repo;
    private readonly IAutorizacionService _autorizacion;

    public  CambiarEstadoExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion)
    {
        _repo = repo;
        _autorizacion = autorizacion;
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

        return new CambiarEstadoExpedienteResponse(expediente.Id,expediente.Estado,expediente.FechaUltimaModificacion);
    }
}
