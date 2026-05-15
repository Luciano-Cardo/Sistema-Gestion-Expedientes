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
        {
            throw new AutorizacionException("El usuario no posee la autorizacion");
        }

        var expedienteModificado = _repo.ObtenerPorId(request.Id);
        if(expedienteModificado == null) throw new Exception("No existe ese expediente");

        expedienteModificado.CambiarEstado(request.NuevoEstado,request.Id);

        _repo.Modificar(expedienteModificado);

        return new CambiarEstadoExpedienteResponse(expedienteModificado.Id,expedienteModificado.Estado,expedienteModificado.FechaUltimaModificacion);
    }
}
