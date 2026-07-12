using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.SQLite;

namespace SGE.Infraestructura;

public class TramiteRepository : ITramiteRepository
{
    private readonly SgeContext _context;

    public TramiteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Tramite tramite)
    {
        _context.Tramites.Add(tramite);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return _context.Tramites.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return _context.Tramites.Where(t => t.ExpedienteId == expedienteId).ToList();
    }

    public void Modificar(Tramite tramite)
    {
        _context.Tramites.Update(tramite);
    }

    public void Eliminar(Guid idTramiteUsuario)
    {
        var tramite = _context.Tramites.FirstOrDefault(t => t.Id == idTramiteUsuario);
        if (tramite == null)
        {
            throw new EntidadNoEncontradaException($"No existe un tramite con el ID {idTramiteUsuario}");
        }

        _context.Tramites.Remove(tramite);
    }
}