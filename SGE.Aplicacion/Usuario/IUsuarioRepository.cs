using SGE.Dominio.Entidades;

namespace SGE.Aplicacion.Interfaces;

public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    Usuario? ObtenerPorCorreo(string correoElectronico);
    Usuario? ObtenerPorId(Guid id);
    List<Usuario> ObtenerTodos();
    void Eliminar(Usuario usuario);
}