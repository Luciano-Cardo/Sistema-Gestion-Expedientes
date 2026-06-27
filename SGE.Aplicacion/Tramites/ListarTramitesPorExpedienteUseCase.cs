using SGE.Aplicacion.Interfaces;
 
namespace SGE.Aplicacion.Tramites;
 
public class ListarTramitesPorExpedienteUseCase
{
    private readonly ITramiteRepository _repo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public ListarTramitesPorExpedienteUseCase(ITramiteRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repo = repo;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
 
    public IEnumerable<TramiteResumenResponse> Ejecutar(ListarTramitesPorExpedienteRequest request)
    {
        _unidadDeTrabajo.Guardar();
        return _repo.ObtenerPorExpedienteId(request.ExpedienteId).Select
            (t => new TramiteResumenResponse(t.Id, t.ExpedienteId, t.Etiqueta, t.Contenido, t.FechaCreacion));
    }
}