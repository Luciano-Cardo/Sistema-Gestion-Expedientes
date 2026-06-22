using SGE.Dominio.Entidades;
using SGE.Dominio.Entidades;

public interface IUsuarioRepository
{
    void Agregar(Usuario Usuario);
    Usuario? obtenerPorCorreo(String CorreoElectronico);
}