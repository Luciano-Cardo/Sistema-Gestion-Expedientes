using SGE.Dominio.Entidades;


namespace SGE.Aplicacion.Interfaces;
public interface IUsuarioRepository
{
    void Agregar(Usuario Usuario);
    Usuario? ObtenerPorCorreo(String CorreoElectronico);
    Usuario? ObtenerPorId(Guid Id);

    IEnumerable<Usuario> ObtenerTodos();
    void Eliminar(Usuario Usuario);
    
}