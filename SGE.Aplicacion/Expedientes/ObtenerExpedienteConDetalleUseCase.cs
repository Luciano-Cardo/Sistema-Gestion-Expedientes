namespace SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
using System.Linq;
public class ObtenerExpedienteConDetalleUseCase
{
    public readonly IExpedienteRepository _expRepo;
    public readonly ITramiteRepository _traRepo;

    public ObtenerExpedienteConDetalleUseCase(IExpedienteRepository expRepo, ITramiteRepository traRepo)
    {
        _expRepo = expRepo;
        _traRepo = traRepo;
    }

    public ObtenerExpedienteConDetalleResponse Ejecutar(ObtenerExpedienteConDetalleRequest request)
    {
        var expediente = _expRepo.ObtenerPorId(request.Id);
        if(expediente == null)
        {
            throw new EntidadNoEncontradaException("No Existe un Expediente con ese ID");
        }
        var tramites = _traRepo.ObtenerPorExpedienteId(request.Id);
        var tramitesDTOs = tramites.Select(t => new TramiteDtos(t.Id,t.Etiqueta.ToString(),t.Contenido.ToString(),t.FechaCreacion)).ToList();
        return new ObtenerExpedienteConDetalleResponse(expediente.Id,expediente.Caratula.ToString(),expediente.Estado.ToString(),expediente.UsuarioUltimoCambio,tramitesDTOs);
    }
}