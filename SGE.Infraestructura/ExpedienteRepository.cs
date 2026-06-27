using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.SQLite;

namespace SGE.Infraestructura;

public class ExpedienteRepository : IExpedienteRepository
{
    private readonly SgeContext _context;

    public ExpedienteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Expediente expediente)
    {
        _context.Expedientes.Add(expediente);
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        return _context.Expedientes.FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        return _context.Expedientes.ToList();
    }

    public void Modificar(Expediente expediente)
    {
        _context.Expedientes.Update(expediente);
    }

    public void Eliminar(Guid id)
    {
        var expediente = _context.Expedientes.FirstOrDefault(e => e.Id == id);
        if (expediente != null)
        {
            _context.Expedientes.Remove(expediente);
        }
    }
}