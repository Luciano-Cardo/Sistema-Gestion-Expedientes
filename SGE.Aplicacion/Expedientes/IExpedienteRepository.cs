using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);

    void Modificar(Guid idExpediente, Expediente nuevoExpediente);

    void Eliminar(Guid idExpediente);

    Expediente? ObtenerPorID(Guid idExpediente);
    
    List<Expediente> ObtenerTodos();
}
