using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AgregarExpedienteUseCase
{
    private IExpedienteRepository _repo;
    private IAutorizacionService _autorizacion;
    public AgregarExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion)
    {
        _repo = repo;
        _autorizacion = autorizacion;
    }

    public void Ejecutar(Expediente expediente, Guid idUsuario)
    {
        if (!_autorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteAlta))
        {
           throw new AutorizacionException("El usuario no posee la autorizacion");
        }

        _repo.Agregar(expediente);



    }
}