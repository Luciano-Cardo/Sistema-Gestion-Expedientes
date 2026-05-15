using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Interfaces;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    Tramite? ObtenerPorId(Guid id);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
    void Modificar(Tramite tramite);
    void Eliminar(Guid idTramiteUsuario);
}
