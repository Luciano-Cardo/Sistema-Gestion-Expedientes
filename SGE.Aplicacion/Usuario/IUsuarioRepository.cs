using SGE.Dominio.Entidades;
namespace SGE.Aplicacion.Interfaces;
public interface IUsuarioRepository
{
    void Agregar(Usuario Usuario);
    Usuario? obtenerPorCorreo(String CorreoElectronico);
    Usuario? obtenerPorId(Guid Id);

    List<Usuario> ObtenerTodos();
    void Eliminar(Usuario Usuario);
    
}