

using System.Runtime.CompilerServices;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class AgregarExpedienteUseCase
{
    private IExpedienteRepository repo;

    public AgregarExpedienteUseCase(IExpedienteRepository _repo)
    {
        repo = _repo;
    }

    public void Ejecutar(Expediente nuevoExpediente)
    {
        




       
    }
}
