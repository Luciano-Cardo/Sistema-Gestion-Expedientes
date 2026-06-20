namespace SGE.Aplicacion.Expedientes;
 
public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _repo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public ListarExpedientesUseCase(IExpedienteRepository repo, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repo = repo;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
 
    public IEnumerable<ExpedienteResumenResponse> Ejecutar(ListarExpedientesRequest request)
    {
        _unidadDeTrabajo.Guardar();
        return _repo.ObtenerTodos().Select
            (e => new ExpedienteResumenResponse(e.Id, e.Caratula, e.Estado, e.FechaCreacion, e.FechaUltimaModificacion, e.UsuarioUltimoCambio));
    }
}