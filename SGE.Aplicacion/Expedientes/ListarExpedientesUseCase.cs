namespace SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _repo;
    public ListarExpedientesUseCase(IExpedienteRepository repo)
    {
        _repo = repo;
    }
 
    public IEnumerable<ExpedienteResumenResponse> Ejecutar(ListarExpedientesRequest request)
    {
        return _repo.ObtenerTodos().Select
            (e => new ExpedienteResumenResponse(e.Id, e.Caratula, e.Estado, e.FechaCreacion, e.FechaUltimaModificacion, e.UsuarioUltimoCambio));
    }
}