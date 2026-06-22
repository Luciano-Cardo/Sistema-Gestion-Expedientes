using SGE.Dominio.Entidades;
<<<<<<< HEAD
using SGE.Dominio.Entidades;

=======
namespace SGE.Aplicacion.Interfaces;
>>>>>>> 8011cb158bd6994018b455084fbc0d202c758687
public interface IUsuarioRepository
{
    void Agregar(Usuario Usuario);
    Usuario? obtenerPorCorreo(String CorreoElectronico);
    Usuario? obtenerPorId(Guid Id);

    List<Usuario> ObtenerTodos();
    void Eliminar(Usuario Usuario);
    
}