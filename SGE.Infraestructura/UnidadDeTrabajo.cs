using SGE.Aplicacion.Interfaces;
using SGE.Infraestructura.SQLite; 

namespace SGE.Infraestructura;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SgeContext _context;

    public UnidadDeTrabajo(SgeContext context)
    {
        _context = context;
    }

    public void Guardar()
    {
        _context.SaveChanges();
    }
}