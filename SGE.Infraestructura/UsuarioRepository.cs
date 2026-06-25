using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Entidades; // O el namespace donde tengas tu clase Usuario
using SGE.Infraestructura.SQLite;
using SGE.Aplicacion.Usuarios;
namespace SGE.Infraestructura;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SgeContext _context;

    public UsuarioRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario); // Asumiendo que tu DbSet se llama Usuarios
    }

    public Usuario? ObtenerPorCorreo(string correoElectronico)
    {
        return _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico == correoElectronico);
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }

    public void Eliminar(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
    }
}