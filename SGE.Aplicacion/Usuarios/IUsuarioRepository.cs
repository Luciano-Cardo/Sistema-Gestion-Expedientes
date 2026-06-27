using SGE.Dominio.Entidades;


namespace SGE.Aplicacion.Interfaces;
public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    Usuario? ObtenerPorCorreo(String correoElectronico);
    Usuario? ObtenerPorId(Guid id);

    IEnumerable<Usuario> ObtenerTodos();
    void Eliminar(Usuario usuario);
    
}