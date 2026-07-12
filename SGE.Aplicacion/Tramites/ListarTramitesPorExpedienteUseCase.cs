using SGE.Aplicacion.Interfaces;
 
namespace SGE.Aplicacion.Tramites;
 
public class ListarTramitesPorExpedienteUseCase
{
    private readonly ITramiteRepository _repo;
    public ListarTramitesPorExpedienteUseCase(ITramiteRepository repo)
    {
        _repo = repo;
    }
 
    public IEnumerable<TramiteResumenResponse> Ejecutar(ListarTramitesPorExpedienteRequest request)
    {
        return _repo.ObtenerPorExpedienteId(request.ExpedienteId).Select
            (t => new TramiteResumenResponse(t.Id, t.ExpedienteId, t.Etiqueta, t.Contenido, t.FechaCreacion));
    }
}